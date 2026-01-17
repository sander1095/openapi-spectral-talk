#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys the Aspire application and registers the API in Azure API Center.

.DESCRIPTION
    This script performs the following operations:
    1. Runs 'aspire deploy' to deploy the application infrastructure
    2. Registers the OpenAPI specification in Azure API Center
    
.PARAMETER ResourceGroupName
    The name of the Azure resource group where resources are deployed

.PARAMETER ApiCenterName
    The name of the Azure API Center instance

.PARAMETER ApiId
    The unique identifier for the API in API Center (default: users-api)

.EXAMPLE
    .\deploy.ps1 -ResourceGroupName "rg-openapi-spectral" -ApiCenterName "apic-api-linting"
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string]$ResourceGroupName = "rg-api-linting",

    [Parameter(Mandatory = $false)]
    [string]$ApiCenterName = "apic-api-linting",

    [Parameter(Mandatory = $false)]
    [string]$ApiId = "users-api"
)

$ErrorActionPreference = "Stop"

# Script directory and OpenAPI file path
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$OpenApiFile = Join-Path (Split-Path -Parent $ScriptDir) "openapi.json"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Aspire Deployment and API Registration Script" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Validate OpenAPI file exists
if (-not (Test-Path $OpenApiFile)) {
    Write-Error "OpenAPI file not found at: $OpenApiFile"
    exit 1
}

Write-Host "✓ OpenAPI file found: $OpenApiFile" -ForegroundColor Green
Write-Host ""

# Step 1: Run aspire deploy
Write-Host "Step 1: Deploying Aspire application..." -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

try {
    # Change to the AppHost directory
    $AppHostDir = Join-Path (Split-Path -Parent $ScriptDir) "src\Aspire.AppHost"
    Push-Location $AppHostDir
    
    Write-Host "Running 'aspire deploy' from: $AppHostDir" -ForegroundColor Gray
    aspire deploy
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Aspire deployment failed with exit code: $LASTEXITCODE"
        Pop-Location
        exit $LASTEXITCODE
    }
    
    Pop-Location
    Write-Host "✓ Aspire deployment completed successfully" -ForegroundColor Green
    Write-Host ""
}
catch {
    Pop-Location
    Write-Error "Error during Aspire deployment: $_"
    exit 1
}

# Step 2: Register API in Azure API Center
Write-Host "Step 2: Registering API in Azure API Center..." -ForegroundColor Yellow
Write-Host "----------------------------------------" -ForegroundColor Yellow

# Check if Azure CLI is installed
Write-Host "Checking Azure CLI installation..." -ForegroundColor Gray
try {
    $azVersion = az version --output json | ConvertFrom-Json
    Write-Host "✓ Azure CLI version: $($azVersion.'azure-cli')" -ForegroundColor Green
}
catch {
    Write-Error "Azure CLI is not installed. Please install it from: https://aka.ms/installazurecliwindows"
    exit 1
}

# Check if logged in to Azure
Write-Host "Checking Azure login status..." -ForegroundColor Gray
try {
    $account = az account show 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Not logged in to Azure. Initiating login..." -ForegroundColor Yellow
        az login
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Azure login failed"
            exit 1
        }
    }
    Write-Host "✓ Logged in to Azure" -ForegroundColor Green
}
catch {
    Write-Error "Error checking Azure login: $_"
    exit 1
}

# Verify the API Center exists
Write-Host "Verifying API Center existence..." -ForegroundColor Gray
try {
    $apiCenterExists = az apic show `
        --resource-group $ResourceGroupName `
        --name $ApiCenterName `
        --output json 2>&1
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "API Center '$ApiCenterName' not found in resource group '$ResourceGroupName'"
        exit 1
    }
    Write-Host "✓ API Center verified" -ForegroundColor Green
}
catch {
    Write-Error "Error verifying API Center: $_"
    exit 1
}

# Read OpenAPI spec to get version info
Write-Host "Reading OpenAPI specification..." -ForegroundColor Gray
try {
    $openApiSpec = Get-Content $OpenApiFile -Raw | ConvertFrom-Json
    $apiTitle = $openApiSpec.info.title
    $specVersion = $openApiSpec.openapi
    
    # Extract API version from OpenAPI spec and convert to API Center format
    # e.g., "1.0.0" becomes "v1-0-0"
    $apiVersionFromSpec = $openApiSpec.info.version
    $ApiVersion = "v" + ($apiVersionFromSpec -replace '\.', '-')
    
    Write-Host "✓ API Title: $apiTitle" -ForegroundColor Green
    Write-Host "✓ OpenAPI Version: $specVersion" -ForegroundColor Green
    Write-Host "✓ API Version (from spec): $ApiVersion" -ForegroundColor Green
}
catch {
    Write-Error "Error reading OpenAPI specification: $_"
    exit 1
}

Write-Host ""
Write-Host "Registering API with the following details:" -ForegroundColor Cyan
Write-Host "  Resource Group: $ResourceGroupName" -ForegroundColor Gray
Write-Host "  API Center: $ApiCenterName" -ForegroundColor Gray
Write-Host "  API ID: $ApiId" -ForegroundColor Gray
Write-Host "  API Title: $apiTitle" -ForegroundColor Gray
Write-Host "  Version ID: $ApiVersion" -ForegroundColor Gray
Write-Host ""

# Create or update the API
Write-Host "Creating/updating API..." -ForegroundColor Gray
try {
    az apic api create `
        --resource-group $ResourceGroupName `
        --service-name $ApiCenterName `
        --api-id $ApiId `
        --title "$apiTitle" `
        --type "rest" `
        --output none
    
    Write-Host "✓ API created/updated" -ForegroundColor Green
}
catch {
    Write-Error "Error creating API: $_"
    exit 1
}

# Create or update the API version
Write-Host "Creating/updating API version..." -ForegroundColor Gray
try {
    az apic api version create `
        --resource-group $ResourceGroupName `
        --service-name $ApiCenterName `
        --api-id $ApiId `
        --version-id $ApiVersion `
        --title "$ApiVersion" `
        --lifecycle-stage "production" `
        --output none
    
    Write-Host "✓ API version created/updated" -ForegroundColor Green
}
catch {
    Write-Error "Error creating API version: $_"
    exit 1
}

# Create the API definition
Write-Host "Creating API definition..." -ForegroundColor Gray
try {
    az apic api definition create `
        --resource-group $ResourceGroupName `
        --service-name $ApiCenterName `
        --api-id $ApiId `
        --version-id $ApiVersion `
        --definition-id "openapi" `
        --title "OpenAPI" `
        --output none
    
    Write-Host "✓ API definition created" -ForegroundColor Green
}
catch {
    Write-Error "Error creating API definition: $_"
    exit 1
}

# Import the OpenAPI specification
Write-Host "Importing OpenAPI specification..." -ForegroundColor Gray
try {
    $specJson = '{\"name\":\"openapi\",\"version\":\"' + $specVersion + '\"}'
    
    az apic api definition import-specification `
        --resource-group $ResourceGroupName `
        --service-name $ApiCenterName `
        --api-id $ApiId `
        --version-id $ApiVersion `
        --definition-id "openapi" `
        --format "inline" `
        --value "@$OpenApiFile" `
        --specification $specJson `
        --output none
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to import OpenAPI specification"
        exit 1
    }
    
    Write-Host "✓ OpenAPI specification imported" -ForegroundColor Green
}
catch {
    Write-Error "Error importing specification: $_"
    exit 1
}

Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "✓ Deployment and API registration completed!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "API registered in Azure API Center:" -ForegroundColor Cyan
Write-Host "  API ID: $ApiId" -ForegroundColor Gray
Write-Host "  Version: $ApiVersion" -ForegroundColor Gray
Write-Host "  Definition: openapi" -ForegroundColor Gray
Write-Host ""
Write-Host "View your API in the Azure Portal:" -ForegroundColor Cyan
Write-Host "  https://portal.azure.com/#resource/subscriptions/<subscription-id>/resourceGroups/$ResourceGroupName/providers/Microsoft.ApiCenter/services/$ApiCenterName/apis/$ApiId" -ForegroundColor Gray
Write-Host ""

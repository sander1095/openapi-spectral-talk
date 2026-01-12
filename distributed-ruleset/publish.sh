#!/bin/bash

# Script to manually publish the ruleset to GitHub Packages
# This requires a GitHub token with package write permissions

set -e

echo "🚀 Publishing OpenAPI Spectral Ruleset to GitHub Packages"
echo ""

# Check if we're in the right directory
if [ ! -f "package.json" ]; then
    echo "❌ Error: package.json not found. Please run this script from the distributed-ruleset directory."
    exit 1
fi

# Check if GitHub token is available
if [ -z "$GITHUB_TOKEN" ]; then
    echo "❌ Error: GITHUB_TOKEN environment variable is not set."
    echo ""
    echo "Please set your GitHub token with package write permissions:"
    echo "  export GITHUB_TOKEN=your_github_token_here"
    echo ""
    echo "You can create a token at: https://github.com/settings/tokens"
    echo "Required permissions: write:packages, read:packages"
    exit 1
fi

# Create .npmrc for authentication
echo "📝 Configuring npm authentication..."
cat > .npmrc << EOF
@sander1095:registry=https://npm.pkg.github.com
//npm.pkg.github.com/:_authToken=${GITHUB_TOKEN}
EOF

echo "✅ Authentication configured"
echo ""

# Publish the package
echo "📦 Publishing package..."
npm publish

echo ""
echo "✅ Package published successfully!"
echo ""
echo "You can now install it with:"
echo "  npm install @sander1095/openapi-spectral-ruleset"

# Clean up .npmrc (contains sensitive token)
rm -f .npmrc
echo ""
echo "🧹 Cleaned up authentication file"

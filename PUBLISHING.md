# Publishing and Consuming the Distributed Ruleset

This guide explains how to publish the distributed Spectral ruleset to GitHub Packages and consume it in your projects.

## Table of Contents

- [Publishing the Ruleset](#publishing-the-ruleset)
  - [Automatic Publishing via GitHub Actions](#automatic-publishing-via-github-actions)
  - [Manual Publishing](#manual-publishing)
- [Consuming the Published Ruleset](#consuming-the-published-ruleset)
- [Local Development](#local-development)

## Publishing the Ruleset

### Automatic Publishing via GitHub Actions

The easiest way to publish the ruleset is to use the automated GitHub Actions workflow:

1. Make changes to files in the `distributed-ruleset/` folder
2. Commit and push your changes to the `main` branch
3. The `publish-ruleset.yml` workflow will automatically run and publish the package to GitHub Packages

You can also trigger the workflow manually:

1. Go to the "Actions" tab in your GitHub repository
2. Select the "Publish Ruleset Package" workflow
3. Click "Run workflow" and select the branch
4. Click "Run workflow" to start the publishing process

### Manual Publishing

To publish the ruleset package manually from your local machine:

1. **Set up authentication** by creating a GitHub Personal Access Token (PAT):
   - Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
   - Click "Generate new token (classic)"
   - Give it a name like "NPM Package Publishing"
   - Select the following scopes:
     - `write:packages` - to publish packages
     - `read:packages` - to download packages
   - Click "Generate token" and copy it

2. **Configure NPM authentication** by creating/updating your `~/.npmrc` file:
   ```bash
   echo "//npm.pkg.github.com/:_authToken=YOUR_GITHUB_TOKEN" >> ~/.npmrc
   ```
   
   Replace `YOUR_GITHUB_TOKEN` with the token you created in step 1.

3. **Navigate to the ruleset directory**:
   ```bash
   cd distributed-ruleset
   ```

4. **Publish the package**:
   ```bash
   npm publish
   ```

5. **Verify the package was published**:
   - Go to your GitHub repository page
   - Click on "Packages" in the right sidebar
   - You should see `@sander1095/openapi-spectral-ruleset` listed

## Consuming the Published Ruleset

Once the ruleset package is published to GitHub Packages, you can use it in any project:

### 1. Configure NPM to use GitHub Packages

Create or update the `.npmrc` file in your project root:

```
@sander1095:registry=https://npm.pkg.github.com
//npm.pkg.github.com/:_authToken=${NODE_AUTH_TOKEN}
```

The `NODE_AUTH_TOKEN` environment variable will be used for authentication.

### 2. Set up Authentication

**For local development:**

Export your GitHub token as an environment variable:
```bash
export NODE_AUTH_TOKEN=YOUR_GITHUB_TOKEN
```

Or create a `.env` file (don't commit this!):
```
NODE_AUTH_TOKEN=your_github_token_here
```

**For GitHub Actions:**

The workflow automatically has access to `GITHUB_TOKEN` secret, which is used via the `NODE_AUTH_TOKEN` environment variable.

### 3. Install the Package

Update your `package.json` to include the ruleset:

```json
{
  "devDependencies": {
    "@stoplight/spectral-cli": "^6.11.0",
    "@sander1095/openapi-spectral-ruleset": "^1.0.0"
  }
}
```

Then install:
```bash
npm install
```

### 4. Reference the Ruleset in Spectral

Create or update your `.spectral.yml`:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

### 5. Run Spectral

```bash
npx spectral lint your-openapi-file.json
```

## Local Development

During local development, before the package is published, you can reference the local ruleset:

1. **Use a local file path in `package.json`**:
   ```json
   {
     "devDependencies": {
       "@stoplight/spectral-cli": "^6.11.0",
       "@sander1095/openapi-spectral-ruleset": "file:./distributed-ruleset"
     }
   }
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **The `.spectral.yml` reference remains the same**:
   ```yaml
   extends: ["@sander1095/openapi-spectral-ruleset"]
   ```

This allows you to test changes to the ruleset locally before publishing.

## Troubleshooting

### Authentication Issues

If you get a 401 or 404 error when installing the package:

1. Verify your GitHub token has the correct scopes (`read:packages`, `write:packages`)
2. Make sure the `NODE_AUTH_TOKEN` environment variable is set
3. Check that the `.npmrc` file is configured correctly
4. Ensure the package has been published to GitHub Packages

### Package Not Found

If NPM can't find the package:

1. Verify the package name matches exactly: `@sander1095/openapi-spectral-ruleset`
2. Check that the package is public or you have access to it
3. Make sure you're authenticated with GitHub Packages
4. Verify the registry configuration in `.npmrc` points to GitHub Packages

### Spectral Can't Find the Ruleset

If Spectral reports it can't find the ruleset:

1. Make sure you've run `npm install` to install the package
2. Verify the package is listed in `node_modules/@sander1095/`
3. Check that the `main` field in the ruleset's `package.json` points to `ruleset.yaml`
4. Ensure the extends syntax in `.spectral.yml` is correct

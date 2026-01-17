---
agent: Context7-Expert
---

# Spectral Rule Creation Guide

This project uses Spectral to validate OpenAPI specifications. When creating or modifying Spectral rules, follow these guidelines. See the "Using Context7 Efficiently" section below for when to consult external documentation.

## Workflow

1. **Understand the requirement** - What should the rule validate?
2. **Use Context7 for syntax** - Get current Spectral documentation
3. **Create the rule** - Follow YAML structure
4. **Test against openapi.json** - Verify it works
5. **Document in this project** - Add to .spectral.yml with comments

## Using Context7

**Always use Context7** to get current Spectral documentation:

1. **Resolve library**: `get-library-id({ libraryName: "spectral" })`

```
resolve-library-id({ libraryName: "spectral" })
get-library-docs({
  context7CompatibleLibraryID: "/stoplightio/spectral",
  topic: "<specific-topic>"
})
```

**Topics to request:**

- `"rules"` - Rule structure and syntax
- `"jsonpath"` - Path expressions for targeting OpenAPI elements
- `"functions"` - Built-in validation functions (truthy, pattern, casing, etc.)
- `"custom-functions"` - Writing JavaScript validators
- `"rulesets"` - Extending and organizing rules

**Token allocation:**

- Quick syntax check: 2000-3000 tokens
- Standard rule creation: 5000 tokens
- Complex custom functions: 7000-10000 tokens

## Project Standards

### Rule Naming

- Use kebab-case: `operation-summary-required`
- Be descriptive: explain what's being validated

### Severity

- `error` - Blocks CI/CD, must fix
- `warn` - Should fix
- `info` - Suggestions
- `hint` - Optional improvements

### Testing

Run: `spectral lint openapi.json --ruleset .spectral.yml`

### Best Practices

- Clear descriptions help users fix issues
- Test with valid and invalid cases
- Add comments for complex JSONPath
- Extend `spectral:oas` rather than replace it
- If the rule is complex, consider a custom function.

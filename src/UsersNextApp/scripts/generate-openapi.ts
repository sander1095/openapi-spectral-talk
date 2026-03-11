import { writeFileSync } from "fs";
import { resolve } from "path";
import openApiDocument from "../lib/openapi.js";

// Write the OpenAPI document to the repository root
// When run via `npm run generate-openapi`, cwd is src/UsersNextApp/
const outputPath = resolve(process.cwd(), "../../openapi.json");
const json = JSON.stringify(openApiDocument, null, 2);

writeFileSync(outputPath, json, "utf-8");
console.log(`✅ OpenAPI document written to ${outputPath}`);

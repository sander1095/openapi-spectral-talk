import { ApiReference } from "@scalar/nextjs-api-reference";

/**
 * GET /scalar
 * Serves the Scalar API reference UI.
 */
export const GET = ApiReference({
  url: "/api/openapi.json",
  pageTitle: "UsersApp | API Reference",
});

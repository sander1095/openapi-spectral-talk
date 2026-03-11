import openApiDocument from "@/lib/openapi";
import { NextResponse } from "next/server";

/**
 * GET /api/openapi.json
 * Returns the OpenAPI specification document.
 */
export function GET(): NextResponse {
  return NextResponse.json(openApiDocument, { status: 200 });
}

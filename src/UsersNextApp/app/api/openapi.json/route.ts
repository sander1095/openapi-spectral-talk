import openApiDocument from "@/lib/openapi";
import { NextResponse } from "next/server";

/**
 * GET /api/openapi.json
 * Returns the OpenAPI specification document.
 */
export async function GET(): Promise<NextResponse> {
  return NextResponse.json(openApiDocument, { status: 200 });
}

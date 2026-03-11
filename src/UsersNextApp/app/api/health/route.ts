import { NextResponse } from "next/server";

/**
 * GET /api/health
 * Returns health status of the application.
 */
export async function GET(): Promise<NextResponse> {
  return NextResponse.json({ status: "Healthy" }, { status: 200 });
}

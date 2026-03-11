import { NextResponse } from "next/server";

/**
 * GET /api/alive
 * Returns liveness status of the application.
 */
export async function GET(): Promise<NextResponse> {
  return NextResponse.json({ status: "Alive" }, { status: 200 });
}

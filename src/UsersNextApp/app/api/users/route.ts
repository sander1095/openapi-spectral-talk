import {
  getAllUsers,
  createUser,
  findUserByEmail,
  type ProblemDetails,
  type CreateUserRequest,
} from "@/lib/users";
import { NextRequest, NextResponse } from "next/server";

/**
 * GET /api/users
 * Returns a list of all users in the system.
 */
export function GET(): NextResponse {
  const users = getAllUsers();
  return NextResponse.json(users, { status: 200 });
}

/**
 * POST /api/users
 * Creates a new user with the provided details.
 */
export async function POST(request: NextRequest): Promise<NextResponse> {
  let body: Partial<CreateUserRequest>;

  try {
    body = await request.json();
  } catch {
    const problem: ProblemDetails = {
      title: "Invalid request",
      detail: "Request body must be valid JSON.",
      status: 400,
    };
    return NextResponse.json(problem, { status: 400 });
  }

  if (!body.name?.trim() || !body.email?.trim()) {
    const problem: ProblemDetails = {
      title: "Invalid request",
      detail: "Name and Email are required fields.",
      status: 400,
    };
    return NextResponse.json(problem, { status: 400 });
  }

  if (findUserByEmail(body.email)) {
    const problem: ProblemDetails = {
      title: "Email already exists",
      detail: `A user with email '${body.email}' already exists.`,
      status: 409,
    };
    return NextResponse.json(problem, { status: 409 });
  }

  const user = createUser({ name: body.name.trim(), email: body.email.trim() });
  return NextResponse.json(user, {
    status: 201,
    headers: { Location: `/api/users/${user.id}` },
  });
}

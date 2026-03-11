import {
  getUserById,
  updateUser,
  deleteUser,
  findUserByEmail,
  type ProblemDetails,
  type UpdateUserRequest,
} from "@/lib/users";
import { NextRequest, NextResponse } from "next/server";

type RouteParams = { params: Promise<{ id: string }> };

/**
 * GET /api/users/{id}
 * Returns a single user by their ID.
 */
export async function GET(
  _request: NextRequest,
  { params }: RouteParams
): Promise<NextResponse> {
  const { id } = await params;
  const userId = parseInt(id, 10);

  if (isNaN(userId)) {
    const problem: ProblemDetails = {
      title: "Invalid ID",
      detail: "The user ID must be a valid integer.",
      status: 400,
    };
    return NextResponse.json(problem, { status: 400 });
  }

  const user = getUserById(userId);
  if (!user) {
    const problem: ProblemDetails = {
      title: "User not found",
      detail: `User with ID ${userId} was not found.`,
      status: 404,
    };
    return NextResponse.json(problem, { status: 404 });
  }

  return NextResponse.json(user, { status: 200 });
}

/**
 * PUT /api/users/{id}
 * Updates an existing user's details.
 */
export async function PUT(
  request: NextRequest,
  { params }: RouteParams
): Promise<NextResponse> {
  const { id } = await params;
  const userId = parseInt(id, 10);

  if (isNaN(userId)) {
    const problem: ProblemDetails = {
      title: "Invalid ID",
      detail: "The user ID must be a valid integer.",
      status: 400,
    };
    return NextResponse.json(problem, { status: 400 });
  }

  let body: Partial<UpdateUserRequest>;
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

  const existing = getUserById(userId);
  if (!existing) {
    const problem: ProblemDetails = {
      title: "User not found",
      detail: `User with ID ${userId} was not found.`,
      status: 404,
    };
    return NextResponse.json(problem, { status: 404 });
  }

  if (findUserByEmail(body.email, userId)) {
    const problem: ProblemDetails = {
      title: "Email already exists",
      detail: `A user with email '${body.email}' already exists.`,
      status: 409,
    };
    return NextResponse.json(problem, { status: 409 });
  }

  const updatedUser = updateUser(userId, {
    name: body.name.trim(),
    email: body.email.trim(),
  });
  return NextResponse.json(updatedUser, { status: 200 });
}

/**
 * DELETE /api/users/{id}
 * Deletes a user from the system.
 */
export async function DELETE(
  _request: NextRequest,
  { params }: RouteParams
): Promise<NextResponse> {
  const { id } = await params;
  const userId = parseInt(id, 10);

  if (isNaN(userId)) {
    const problem: ProblemDetails = {
      title: "Invalid ID",
      detail: "The user ID must be a valid integer.",
      status: 400,
    };
    return NextResponse.json(problem, { status: 400 });
  }

  const user = getUserById(userId);
  if (!user) {
    const problem: ProblemDetails = {
      title: "User not found",
      detail: `User with ID ${userId} was not found.`,
      status: 404,
    };
    return NextResponse.json(problem, { status: 404 });
  }

  deleteUser(userId);
  return new NextResponse(null, { status: 204 });
}

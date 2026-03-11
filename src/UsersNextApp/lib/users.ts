export interface User {
  id: number;
  name: string;
  email: string;
}

export interface CreateUserRequest {
  name: string;
  email: string;
}

export interface UpdateUserRequest {
  name: string;
  email: string;
}

export interface ProblemDetails {
  title: string;
  detail: string;
  status: number;
  type?: string;
  instance?: string;
}

// In-memory user store with seed data
const users: User[] = [
  { id: 1, name: "John Doe", email: "john@example.com" },
  { id: 2, name: "Jane Smith", email: "jane@example.com" },
];

let nextId = 3;

export function getAllUsers(): User[] {
  return [...users];
}

export function getUserById(id: number): User | undefined {
  return users.find((u) => u.id === id);
}

export function findUserByEmail(
  email: string,
  excludeId?: number
): User | undefined {
  return users.find(
    (u) =>
      u.email.toLowerCase() === email.toLowerCase() &&
      (excludeId === undefined || u.id !== excludeId)
  );
}

export function createUser(request: CreateUserRequest): User {
  const user: User = { id: nextId++, name: request.name, email: request.email };
  users.push(user);
  return user;
}

export function updateUser(id: number, request: UpdateUserRequest): User {
  const index = users.findIndex((u) => u.id === id);
  const updatedUser: User = { id, name: request.name, email: request.email };
  users[index] = updatedUser;
  return updatedUser;
}

export function deleteUser(id: number): void {
  const index = users.findIndex((u) => u.id === id);
  users.splice(index, 1);
}

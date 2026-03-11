const openApiDocument = {
  openapi: "3.1.0",
  info: {
    title: "UsersApp | v1",
    version: "1.0.0",
  },
  paths: {
    "/api/users": {
      get: {
        tags: ["Users"],
        summary: "Get all users",
        description: "Returns a list of all users in the system.",
        responses: {
          "200": {
            description: "OK",
            content: {
              "application/json": {
                schema: {
                  type: "array",
                  items: {
                    $ref: "#/components/schemas/User",
                  },
                },
              },
            },
          },
        },
      },
      post: {
        tags: ["Users"],
        summary: "Create a new user",
        description: "Creates a new user with the provided details.",
        operationId: "CreateUser",
        requestBody: {
          content: {
            "application/json": {
              schema: {
                $ref: "#/components/schemas/CreateUserRequest",
              },
            },
          },
          required: true,
        },
        responses: {
          "201": {
            description: "Created",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/User",
                },
              },
            },
          },
          "400": {
            description: "Bad Request",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
          "409": {
            description: "Conflict",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
        },
      },
    },
    "/api/users/{id}": {
      get: {
        tags: ["Users"],
        summary: "Get a user by ID",
        description: "Returns a single user by their ID.",
        operationId: "GetUserById",
        parameters: [
          {
            name: "id",
            in: "path",
            required: true,
            schema: {
              type: "integer",
              format: "int32",
            },
          },
        ],
        responses: {
          "200": {
            description: "OK",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/User",
                },
              },
            },
          },
          "404": {
            description: "Not Found",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
        },
      },
      put: {
        tags: ["Users"],
        summary: "Update an existing user",
        description: "Updates an existing user's details.",
        operationId: "UpdateUser",
        parameters: [
          {
            name: "id",
            in: "path",
            required: true,
            schema: {
              type: "integer",
              format: "int32",
            },
          },
        ],
        requestBody: {
          content: {
            "application/json": {
              schema: {
                $ref: "#/components/schemas/UpdateUserRequest",
              },
            },
          },
          required: true,
        },
        responses: {
          "200": {
            description: "OK",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/User",
                },
              },
            },
          },
          "400": {
            description: "Bad Request",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
          "404": {
            description: "Not Found",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
          "409": {
            description: "Conflict",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
        },
      },
      delete: {
        tags: ["Users"],
        summary: "Delete a user",
        description: "Deletes a user from the system.",
        operationId: "DeleteUser",
        parameters: [
          {
            name: "id",
            in: "path",
            required: true,
            schema: {
              type: "integer",
              format: "int32",
            },
          },
        ],
        responses: {
          "204": {
            description: "No Content",
          },
          "404": {
            description: "Not Found",
            content: {
              "application/json": {
                schema: {
                  $ref: "#/components/schemas/ProblemDetails",
                },
              },
            },
          },
        },
      },
    },
  },
  components: {
    schemas: {
      User: {
        required: ["id", "name", "email"],
        type: "object",
        properties: {
          id: {
            type: "integer",
            format: "int32",
          },
          name: {
            type: "string",
          },
          email: {
            type: "string",
          },
        },
      },
      CreateUserRequest: {
        required: ["name", "email"],
        type: "object",
        properties: {
          name: {
            type: "string",
          },
          email: {
            type: "string",
          },
        },
      },
      UpdateUserRequest: {
        required: ["name", "email"],
        type: "object",
        properties: {
          name: {
            type: "string",
          },
          email: {
            type: "string",
          },
        },
      },
      ProblemDetails: {
        type: "object",
        properties: {
          type: {
            type: "string",
            nullable: true,
          },
          title: {
            type: "string",
            nullable: true,
          },
          status: {
            type: "integer",
            format: "int32",
            nullable: true,
          },
          detail: {
            type: "string",
            nullable: true,
          },
          instance: {
            type: "string",
            nullable: true,
          },
        },
      },
    },
  },
  tags: [
    {
      name: "Users",
    },
  ],
};

export default openApiDocument;

import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "UsersApp",
  description: "A Next.js Users API with OpenAPI documentation",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}

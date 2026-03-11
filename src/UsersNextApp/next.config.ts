import type { NextConfig } from "next";
import path from "path";

const nextConfig: NextConfig = {
  // Set the repo root as the output file tracing root to avoid workspace detection warnings
  outputFileTracingRoot: path.resolve(__dirname, "../.."),
};

export default nextConfig;

import React from "react";
import ReactDOM from "react-dom/client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";

import Router from "./Router";

export const API_URL = "";
// export const API_URL = "https://localhost:7024";

export const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <Router />
    </QueryClientProvider>
  </React.StrictMode>
);

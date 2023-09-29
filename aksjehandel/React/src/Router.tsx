import { useEffect, useState } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";

import {
  useQuery,
  useMutation,
  useQueryClient,
  QueryClient,
  QueryClientProvider,
} from "@tanstack/react-query";

import App from "./App";
import Home from "./pages/Home";
import Login from "./pages/Login";
import NotFound from "./pages/NotFound";
import Purchase from "./pages/Purchase";
import Sell from "./pages/Sell";
import Signup from "./pages/Signup";
import { GetUser, User, UserContext } from "./User";
import { Stock, StockPurchase } from "./types";
import { getAssets, getStocks } from "./lib";
import Profile from "./pages/Profile";

export default function Router() {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => GetUser(setUser), []);

  const { data: stocks } = useQuery<Stock[]>(["stocks"], () => getStocks(), {
    initialData: [],
  });

  const { data: assets } = useQuery<StockPurchase[]>(
    ["assets", user?.id],
    () => getAssets(),
    {
      initialData: [],
      enabled: !!user,
    }
  );

  return (
    <UserContext.Provider value={user}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<App />}>
            <Route index element={<Home stocks={stocks} assets={assets} />} />

            <Route
              path="/purchase"
              element={<Purchase stocks={stocks} assets={assets} />}
            />
            <Route
              path="/sell"
              element={<Sell stocks={stocks} assets={assets} />}
            />
            <Route path="/login" element={<Login setUser={setUser} />} />
            <Route path="/signup" element={<Signup />} />
            <Route path="/profile" element={<Profile setUser={setUser} />} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </UserContext.Provider>
  );
}

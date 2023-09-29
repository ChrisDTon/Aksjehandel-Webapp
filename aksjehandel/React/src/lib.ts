import { API_URL } from "./main";
import { Stock, StockPurchase } from "./types";

export const getStocks = (): Promise<Stock[]> =>
  fetch(`${API_URL}/api/stock`).then((res) => {
    if (!res.ok) throw new Error("Failed to fetch stocks");

    return res.json();
  });

export const getAssets = (): Promise<StockPurchase[]> =>
  fetch(`${API_URL}/api/purchase`).then((res) => {
    if (!res.ok) throw new Error("Failed to fetch assets");

    return res.json();
  });

export const validation = {
  name: new RegExp("^[a-zA-ZæøåÆØÅ ]{1,35}$"),
  email: new RegExp("^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$"),
};

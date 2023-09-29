export interface Stock {
  id: number;
  name: string;
  price: number;
  change: number;
  market_cap: number;
}

export interface StockPurchase {
  id: number;
  price: number;
  quantity: number;
  stock_id: number;
}

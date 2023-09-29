import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { API_URL } from "../main";
import { Stock } from "../types";

interface Props {
  stocks: Stock[];
}

function Stocks({ stocks }: Props) {
  return (
    <div>
      <h3>Stocks</h3>

      <table className="table" id="stocks">
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Name</th>
            <th scope="col">Price</th>
            <th scope="col">Change</th>
            <th scope="col">Market Cap</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          {stocks.map((stock) => (
            <tr key={stock.id}>
              <th scope="row">{stock.id}</th>
              <td>{stock.name}</td>
              <td>${stock.price.toLocaleString()}</td>
              <td
                style={{
                  color: stock.change >= 0 ? "green" : "red",
                }}
              >
                {stock.change.toFixed(2)}
              </td>
              <td>${stock.market_cap.toLocaleString()}B</td>
              <td>
                <Link to={{ pathname: "/purchase", search: `?id=${stock.id}` }}>
                  <button type="button" className="btn btn-sm btn-success">
                    Buy
                  </button>
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Stocks;

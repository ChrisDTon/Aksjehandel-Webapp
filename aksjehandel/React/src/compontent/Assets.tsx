import { useContext } from "react";
import { Link } from "react-router-dom";
import { API_URL, queryClient } from "../main";
import { Stock, StockPurchase } from "../types";
import { UserContext } from "../User";

import toast from "react-hot-toast";

interface Props {
  stocks: Stock[];
  assets: StockPurchase[];
}

function Assets({ stocks, assets }: Props) {
  const user = useContext(UserContext);

  const sellAll = async (assetId: number) => {
    if (!user) return;

    const asset = assets.find((a) => a.id === assetId);
    if (!asset) return;

    const stock = stocks.find((a) => a.id === asset.stock_id);
    if (!stock) return;

    const confirmed = confirm(
      `Are you sure you want to sell all your ${stock.name} stocks?`
    );

    if (!confirmed) return;

    const res = await fetch(`${API_URL}/api/purchase/${asset.id}`, {
      method: "DELETE",
    });

    if (!res.ok) {
      toast.error(`Failed to sell ${stock.name} stocks`);
      return;
    }

    queryClient.invalidateQueries(["assets", user.id]);
    toast.success(`Sold all ${stock.name} stocks`);
  };

  return (
    <div>
      <h3>Assets</h3>

      {!user && (
        <p>
          Please <Link to="login">login</Link> to view your assets
        </p>
      )}
      {user && (
        <table className="table" id="stocks">
          <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">Name</th>
              <th scope="col">Price</th>
              <th scope="col">Quantity</th>
              <th scope="col">Total</th>
              <th scope="col"></th>
            </tr>
          </thead>
          <tbody>
            {stocks &&
              assets &&
              assets.map((asset) => {
                const stock = stocks.find((s) => s.id === asset.stock_id);

                if (!stock) return null;

                return (
                  <tr key={asset.id}>
                    <th scope="row">{asset.id}</th>
                    <td>{stock.name}</td>
                    <td>${asset.price.toLocaleString()}</td>
                    <td>{asset.quantity.toLocaleString()}</td>
                    <td>${(asset.price * asset.quantity).toLocaleString()}</td>
                    <td>
                      <div className="button-group">
                        <Link
                          to={{ pathname: "/sell", search: `?id=${asset.id}` }}
                        >
                          <button
                            type="button"
                            className="btn btn-sm btn-primary"
                          >
                            Sell
                          </button>
                        </Link>
                        <button
                          type="button"
                          className="btn btn-sm btn-danger text-decoration-underline"
                          onClick={() => sellAll(asset.id)}
                        >
                          Sell All!
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default Assets;

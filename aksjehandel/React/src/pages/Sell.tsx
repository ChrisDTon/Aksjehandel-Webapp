import { useContext, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate, useSearchParams } from "react-router-dom";
import { API_URL, queryClient } from "../main";
import { Stock, StockPurchase } from "../types";
import { UserContext } from "../User";
import toast from "react-hot-toast";

interface Props {
  stocks: Stock[];
  assets: StockPurchase[];
}

function Sell({ stocks, assets }: Props) {
  const user = useContext(UserContext);

  const nagivation = useNavigate();
  const [searchParams] = useSearchParams();

  const [quantity, setQuantity] = useState(1);
  const [selectedAsset, setSelectedAsset] = useState<number>(
    Number(searchParams.get("id") || 0)
  );

  const sell = async () => {
    if (!selectedAsset || !quantity || !user) return;

    const currentAsset = assets.find((a) => a.id === selectedAsset);
    if (!currentAsset) return;

    currentAsset.quantity = quantity;

    // Attempt to login
    const response = await fetch(`${API_URL}/api/purchase`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(currentAsset),
    });

    const data = await response.text();

    if (!response.ok) {
      toast.error(data);
      return;
    }

    // Invalid assets
    queryClient.invalidateQueries(["assets", user.id]);

    toast.success("Assets sold!");
    nagivation("/");
  };

  return (
    <div className="bg-white shadow-sm p-3 border rounded">
      <h4>Sell Assets</h4>
      <Form>
        <Form.Group className="mb-3" controlId="stock">
          <Form.Label>Asset</Form.Label>
          <Form.Select
            value={selectedAsset}
            onChange={(e) => {
              setSelectedAsset(Number(e.target.value));
            }}
            aria-label="Select asset"
          >
            <option>Select asset</option>

            {assets.map((asset) => {
              const stock = stocks.find((s) => s.id === asset.stock_id);

              if (!stock) return null;

              return <option value={asset.id}>{stock.name}</option>;
            })}
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3" controlId="quantity">
          <Form.Label>Quantity</Form.Label>

          <Form.Control
            value={quantity}
            type="number"
            onChange={(e) => setQuantity(Number(e.target.value))}
            aria-label="quantity"
            aria-describedby="Quantity"
          />
        </Form.Group>

        {Boolean(selectedAsset) && (
          <div className="mb-3">
            <p>
              Total available:{" "}
              {(
                assets.find((a) => a.id === selectedAsset)?.quantity || 0
              ).toLocaleString()}
            </p>
          </div>
        )}

        <Button
          as="input"
          type="submit"
          value="Confirm sale"
          disabled={selectedAsset === 0 || !user || quantity <= 0}
          onClick={(e) => {
            e.preventDefault();
            sell();
          }}
        />
        {!user && (
          <p className="text-danger mt-2 mb-0">
            You must be logged in to sell assets
          </p>
        )}
      </Form>
    </div>
  );
}

export default Sell;

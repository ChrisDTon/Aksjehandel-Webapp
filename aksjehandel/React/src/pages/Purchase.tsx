import { useContext, useMemo, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate, useSearchParams } from "react-router-dom";
import { API_URL, queryClient } from "../main";
import { Stock, StockPurchase } from "../types";
import { UserContext } from "../User";
import toast from "react-hot-toast";

interface Props {
  stocks: Stock[];
  assets?: StockPurchase[];
}

function Purchase({ stocks, assets }: Props) {
  const user = useContext(UserContext);

  const nagivation = useNavigate();
  const [searchParams] = useSearchParams();

  const [quantity, setQuantity] = useState(1);
  const [selectedStock, setSelectedStock] = useState<number>(
    Number(searchParams.get("id") || 0)
  );

  const [validated, setValidated] = useState(false);

  // Caculate the total asset quantity after purchase
  const total = useMemo(() => {
    const currentAsset = assets?.find(
      (asset) => asset.stock_id === selectedStock
    );

    let count = quantity;

    if (currentAsset) {
      count += currentAsset.quantity;
    }

    return count;
  }, [quantity, selectedStock, assets]);

  const purchase = async (event: any) => {
    if (!selectedStock || !quantity || !user) return;

    setValidated(false);

    const form = event.currentTarget;

    event.preventDefault();
    event.stopPropagation();

    const validForm = form.checkValidity();
    setValidated(true);

    // If invalid form, stop
    if (!validForm) return;

    const stock = stocks.find((s) => s.id === selectedStock);

    if (!stock) return;

    // Attempt to login
    const response = await fetch(`${API_URL}/api/purchase`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        stock_id: selectedStock,
        quantity,
        price: stock.price,
      }),
    });

    const data = await response.text();

    if (!response.ok) {
      toast.error(data);
      return;
    }

    // Invalidate cached assets
    queryClient.invalidateQueries(["assets", user.id]);

    toast.success(`Purchased ${quantity} ${stock.name} stock(s)`);
    nagivation("/");
  };

  return (
    <div className="bg-white shadow-sm p-3 border rounded">
      <h4>Purchase stocks</h4>
      <Form noValidate validated={validated} onSubmit={purchase}>
        <Form.Group className="mb-3" controlId="stock">
          <Form.Label>Stock</Form.Label>
          <Form.Select
            value={selectedStock}
            onChange={(e) => {
              setSelectedStock(Number(e.target.value));
            }}
            aria-label="Select stock"
          >
            <option>Select stock</option>

            {stocks.map((stock) => (
              <option value={stock.id}>{stock.name}</option>
            ))}
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

        <Form.Group className="mb-3" controlId="checkbox">
          <p className="text-muted">
            I accept www.aksjehandel.org's{" "}
            <a href="www.aksjehandel.org" target="_blank">
              terms and conditions
            </a>{" "}
            of trade
          </p>
          <Form.Check
            required
            label="Agree to terms and conditions"
            feedback="You must agree before submitting."
            feedbackType="invalid"
          />
        </Form.Group>

        {Boolean(selectedStock) && (
          <div className="mb-3">
            <p>Total quantity after purchase: {total.toLocaleString()}</p>
          </div>
        )}

        <Button
          as="input"
          type="submit"
          value="Confirm purchase"
          disabled={selectedStock === 0 || !user || quantity <= 0}
        />
        {!user && (
          <p className="text-danger mt-2 mb-0">
            You must be logged in to purchase stocks
          </p>
        )}
      </Form>
    </div>
  );
}

export default Purchase;

import Stocks from "../compontent/Stocks";
import Assets from "../compontent/Assets";
import { useContext } from "react";
import { UserContext } from "../User";
import { Stock, StockPurchase } from "../types";

interface Props {
  stocks: Stock[];
  assets: StockPurchase[];
}

function Home({ stocks, assets }: Props) {
  const user = useContext(UserContext);

  return (
    <>
      {!user && <p>You are not logged in!</p>}
      {user && (
        <p>
          Welcome {user.firstname} {user.lastname}!
        </p>
      )}

      <div className="row">
        <div className="col bg-white shadow-sm p-3 me-1 mb-2 border rounded">
          <Stocks stocks={stocks} />
        </div>
        <div className="col bg-white shadow-sm p-3 me-1 mb-2 border rounded">
          <Assets stocks={stocks} assets={assets} />
        </div>
      </div>
    </>
  );
}

export default Home;

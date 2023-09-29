import { useContext, useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { API_URL } from "../main";
import { Logout, UserContext } from "../User";

function Navigation() {
  const user = useContext(UserContext);
  const location = useLocation();

  const [path, setPath] = useState<string>("");

  useEffect(() => {
    const curPath = window.location.pathname.split("/")[1];
    setPath(curPath);
  }, [location]);

  const logout = async () => {
    await Logout();

    window.location.href = "/";
  };

  return (
    <div className="bg-white shadow-sm">
      <div className="container">
        <header className="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 mb-4">
          <Link
            to="/"
            className="d-flex align-items-center col-md-3 mb-2 mb-md-0 text-dark text-decoration-none"
          >
            <h2>Stock Trading</h2>
          </Link>

          <ul className="nav col-12 col-md-auto mb-2 justify-content-center mb-md-0">
            <li>
              <Link
                to="/"
                className={`nav-link px-2 ${
                  !path ? "link-dark" : "link-secondary"
                }`}
              >
                Home
              </Link>
            </li>
            <li>
              <Link
                to="/purchase"
                className={`nav-link px-2 ${
                  path === "purchase" ? "link-dark" : "link-secondary"
                }`}
              >
                Purchase
              </Link>
            </li>
            <li>
              <Link
                to="/sell"
                className={`nav-link px-2 ${
                  path === "sell" ? "link-dark" : "link-secondary"
                }`}
              >
                Sell
              </Link>
            </li>
            {user && (
              <li>
                <Link
                  to="/profile"
                  className={`nav-link px-2 ${
                    path === "profile" ? "link-dark" : "link-secondary"
                  }`}
                >
                  Profile
                </Link>
              </li>
            )}
          </ul>

          <div className="nav align-items-center gap-2">
            {user && (
              <>
                <span>
                  Logged in as {user.lastname}, {user.firstname}
                </span>
                <button
                  type="button"
                  className="btn btn-outline-primary"
                  onClick={() => logout()}
                >
                  Logout
                </button>
              </>
            )}

            {!user && (
              <>
                <Link to="/login">
                  <button
                    type="button"
                    className={`btn btn-outline-primary me-2 ${
                      path === "login" ? "active" : ""
                    }`}
                  >
                    Login
                  </button>
                </Link>
                <Link to="/signup">
                  <button
                    type="button"
                    className={`btn btn-outline-secondary ${
                      path === "signup" ? "active" : ""
                    }`}
                  >
                    Sign-up
                  </button>
                </Link>
              </>
            )}
          </div>
        </header>
      </div>
    </div>
  );
}

export default Navigation;

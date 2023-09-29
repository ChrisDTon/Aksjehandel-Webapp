import { Toaster } from "react-hot-toast";

import { Outlet } from "react-router-dom";
import Navigation from "./compontent/Navbar";

function App() {
  return (
    <>
      <Navigation />
      <div className="container">
        <Toaster
          position="bottom-center"
          toastOptions={{
            duration: 5000,
          }}
        />
        <Outlet />
      </div>
    </>
  );
}

export default App;

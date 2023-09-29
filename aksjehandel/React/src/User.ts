import { createContext } from "react";
import { API_URL } from "./main";

export interface User {
  id: number;
  email: string;
  firstname: string;
  lastname: string;
}

export const UserContext = createContext<User | null>(null);

export const Logout = async () => {
  localStorage.removeItem("user");
  await fetch(`${API_URL}/api/customer/logout`);
};

export const UpdateUser = (
  user: User,
  setUser: React.Dispatch<React.SetStateAction<User | null>>
) => {
  localStorage.setItem("user", JSON.stringify(user));
  setUser(user);
};

export const GetUser = (
  setUser: React.Dispatch<React.SetStateAction<User | null>>
): void => {
  const userData = localStorage.getItem("user");

  if (userData) {
    const user = JSON.parse(userData);
    setUser(user);
  }

  // Fetch data
  fetch(`${API_URL}/api/customer`)
    .then((res) => {
      if (!res.ok) throw Error("Failed to fetch user object");

      return res.json();
    })
    .then((data) => {
      setUser(data);
      localStorage.setItem("user", JSON.stringify(data));
    })
    .catch((error) => {
      console.error(error);
      setUser(null);
    });
};

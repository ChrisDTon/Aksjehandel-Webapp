import { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { API_URL } from "../main";
import { GetUser } from "../User";

import toast from "react-hot-toast";

interface Props {
  setUser: (user: any) => void;
}

function Login(props: Props) {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [loading, setLoading] = useState(false);
  const [validated, setValidated] = useState(false);

  const auth = async (event: any) => {
    setValidated(false);

    const form = event.currentTarget;

    event.preventDefault();
    event.stopPropagation();

    const validForm = form.checkValidity();
    setValidated(true);

    // If invalid form, stop
    if (!validForm) return;

    setLoading(true);

    // Attempt to login
    const response = await fetch(`${API_URL}/api/customer/login`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email,
        password,
      }),
    });

    setLoading(false);

    if (!response.ok) {
      setValidated(false);
      toast.error("Invalid Credentials");
      return;
    }

    toast.success("Login Successful");

    GetUser(props.setUser);
    navigate("/");
  };

  return (
    <div className="bg-white shadow-sm p-3 border rounded">
      <h3>Login</h3>
      <Form noValidate validated={validated} onSubmit={auth}>
        <Form.Group className="mb-3" controlId="formBasicEmail">
          <Form.Label>Email address</Form.Label>
          <Form.Control
            value={email}
            onChange={(e) => setEmail(e.target.value.trim())}
            type="email"
            placeholder="Enter email"
          />
          <Form.Control.Feedback type="invalid">
            Please enter a valid email address
          </Form.Control.Feedback>
        </Form.Group>

        <Form.Group className="mb-3" controlId="formBasicPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="Password"
          />
          <Form.Text className="text-muted">
            Don't have an account? <Link to="/signup">Sign up</Link>
          </Form.Text>
        </Form.Group>

        <Button
          variant="primary"
          type="submit"
          disabled={!email || !password || loading}
        >
          {loading ? "Loading…" : "Login"}
        </Button>
      </Form>
    </div>
  );
}

export default Login;

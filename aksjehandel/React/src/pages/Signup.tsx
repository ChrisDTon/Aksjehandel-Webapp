import { useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { API_URL } from "../main";

import toast from "react-hot-toast";
import { validation } from "../lib";

function Signup() {
  const nagivation = useNavigate();

  const [email, setEmail] = useState("");
  const [firstname, setFirstname] = useState("");
  const [lastname, setLastname] = useState("");
  const [password, setPassword] = useState("");

  const [loading, setLoading] = useState(false);
  const [validated, setValidated] = useState(false);

  const signup = async (event: any) => {
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
    const response = await fetch(`${API_URL}/api/customer/register`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        firstname,
        lastname,
        email,
        password,
      }),
    });

    setLoading(false);

    if (!response.ok) {
      setValidated(false);
      toast.error("Email is already in use");
      return;
    }

    // If successful, redirect to login page
    toast.success("User Created!");
    nagivation("/login");
  };

  return (
    <div className="bg-white shadow-sm p-3 border rounded">
      <h3>Sign Up</h3>
      <Form noValidate validated={validated} onSubmit={signup}>
        <Row className="mb-3">
          <Form.Group as={Col} md="6" controlId="formBasicFirstname">
            <Form.Label>First name</Form.Label>
            <Form.Control
              value={firstname}
              onChange={(e) => setFirstname(e.target.value.trim())}
              type="text"
              placeholder="Enter first name"
              pattern={validation.name.source}
            />
            <Form.Control.Feedback type="invalid">
              Name must be 1 to 35 characters and letters only
            </Form.Control.Feedback>
          </Form.Group>

          <Form.Group as={Col} md="6" controlId="formBasicLastname">
            <Form.Label>Last name</Form.Label>
            <Form.Control
              value={lastname}
              onChange={(e) => setLastname(e.target.value.trim())}
              type="text"
              placeholder="Enter last name"
              pattern={validation.name.source}
            />
            <Form.Control.Feedback type="invalid">
              Name must be 1 to 35 characters and letters only
            </Form.Control.Feedback>
          </Form.Group>
        </Row>

        <Form.Group className="mb-3" controlId="formBasicEmail">
          <Form.Label>Email address</Form.Label>
          <Form.Control
            value={email}
            onChange={(e) => setEmail(e.target.value.trim())}
            type="email"
            placeholder="Enter email"
            pattern={validation.email.source}
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
            Already have an account? <Link to="/login">Login</Link>
          </Form.Text>
        </Form.Group>

        <Button
          variant="primary"
          type="submit"
          disabled={!email || !password || !firstname || !lastname || loading}
        >
          {loading ? "Loading…" : "Sign up"}
        </Button>
      </Form>
    </div>
  );
}

export default Signup;

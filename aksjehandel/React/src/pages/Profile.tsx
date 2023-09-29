import { useContext, useState } from "react";
import { Button, Col, Form, Modal, Row } from "react-bootstrap";
import { API_URL } from "../main";
import { UpdateUser, User, UserContext } from "../User";

import toast from "react-hot-toast";
import { validation } from "../lib";

interface Props {
  setUser: (user: any) => void;
}

function Profile(props: Props) {
  const user = useContext(UserContext);

  if (!user) return <p>You are not logged in!</p>;

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const [email, setEmail] = useState(user.email);
  const [firstname, setFirstname] = useState(user.firstname);
  const [lastname, setLastname] = useState(user.lastname);

  const [newPassword, setNewPassword] = useState("");
  const [validated, setValidated] = useState(false);

  const changePassword = async () => {
    const response = await fetch(`${API_URL}/api/customer/password`, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        password: newPassword,
      }),
    });

    if (!response.ok) {
      toast.error("Failed to update password");
      return;
    }

    setShow(false);
    toast.success("Password updated");
  };

  const update = async (event: any) => {
    setValidated(false);

    const form = event.currentTarget;

    event.preventDefault();
    event.stopPropagation();

    const validForm = form.checkValidity();
    setValidated(true);

    // If invalid form, stop
    if (!validForm) return;

    const response = await fetch(`${API_URL}/api/customer`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        email,
        firstname,
        lastname,
      }),
    });

    if (!response.ok) {
      toast.error("Failed to update profile");
      return;
    }

    UpdateUser({ ...user, email, firstname, lastname }, props.setUser);

    toast.success("Profile updated successfully");
  };

  return (
    <div className="col bg-white shadow-sm p-3 me-1 mb-2 border rounded">
      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Change password</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group className="mb-3" controlId="changePassword">
              <Form.Label>New Password</Form.Label>
              <Form.Control
                type="password"
                placeholder="New Password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" onClick={changePassword}>
            Change password
          </Button>
        </Modal.Footer>
      </Modal>
      <h4>User profile</h4>
      <Form noValidate validated={validated} onSubmit={update}>
        <Row className="mb-3">
          <Form.Group as={Col} md="6" controlId="formBasicEmail">
            <Form.Label>ID</Form.Label>
            <Form.Control type="text" readOnly value={user.id} disabled />
          </Form.Group>
          <Form.Group as={Col} md="6" controlId="email">
            <Form.Label>Email address</Form.Label>
            <Form.Control
              type="email"
              placeholder="Enter email"
              value={email}
              onChange={(e) => setEmail(e.target.value.trim())}
              pattern={validation.email.source}
            />
            <Form.Control.Feedback type="invalid">
              Please enter a valid email address
            </Form.Control.Feedback>
          </Form.Group>
        </Row>
        <Row className="mb-3">
          <Form.Group as={Col} md="6" controlId="firstname">
            <Form.Label>First name</Form.Label>
            <Form.Control
              type="text"
              placeholder="First name"
              value={firstname}
              onChange={(e) => setFirstname(e.target.value.trim())}
              pattern={validation.name.source}
            />
            <Form.Control.Feedback type="invalid">
              Name must be 1 to 35 characters and letters only
            </Form.Control.Feedback>
          </Form.Group>
          <Form.Group as={Col} md="6" controlId="lastname">
            <Form.Label>Last name</Form.Label>
            <Form.Control
              type="text"
              placeholder="Last name"
              value={lastname}
              onChange={(e) => setLastname(e.target.value.trim())}
              pattern={validation.name.source}
            />
            <Form.Control.Feedback type="invalid">
              Name must be 1 to 35 characters and letters only
            </Form.Control.Feedback>
          </Form.Group>
        </Row>
        <div className="d-flex gap-1">
          <Button
            as="input"
            type="submit"
            value="Update profile"
            disabled={!firstname || !lastname || !email}
          />
          <Button
            variant="secondary"
            as="input"
            type="submit"
            value="Change password"
            onClick={(e) => {
              e.preventDefault();
              handleShow();
            }}
          />
        </div>
      </Form>
    </div>
  );
}

export default Profile;

import React, { useState } from 'react';
import { Redirect } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

export const SignIn = ({ setRole }) => {
    const [email, setEmail] = useState("");
    const [passwordHash, setPasswordHash] = useState("");
    const [redirect, setRedirect] = useState(false);

    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => {
        setShow(true);
        setEmail('');
    }

    const handleSubmit = async (event) => {
        event.preventDefault();

        try {
            const response = await fetch("http://localhost:5000/api/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                credentials: 'include',
                body: JSON.stringify({
                    email,
                    passwordHash
                }),
            });

            if (response.ok) {
                const content = await response.json();
                setRedirect(true);
                setRole(content.role);
            } else {
                const errorData = await response.json();
                alert(errorData.message);
                console.log("Sign In error: ", errorData.message);
            }
        } catch (error) {
            console.error("Sign In error: ", error);
        }
    }

    if (redirect) {
        return <Redirect to="/" />;
    }

    const handleSendEmail = async () => {
        try {
            const response = await fetch("http://localhost:5000/api/restore-password", {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: 'include',
                body: JSON.stringify({
                    email
                }),
            });

            if (response.ok) {
                handleClose();
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    return (
        <div>
            <h2>Sign In</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <div className="form-group">
                        <label htmlFor="email">E-mail: </label>
                        <input
                            type="email"
                            className="form-control"
                            id="email"
                            name="email"
                            onChange={e => setEmail(e.target.value)}
                            placeholder="example@example.com"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Password: </label>
                        <input
                            type="password"
                            className="form-control"
                            id="passwordHash"
                            name="passwordHash"
                            onChange={e => setPasswordHash(e.target.value)}
                            placeholder="*******"
                            required
                        />
                    </div>
                    <div>
                        <button type="submit" className="btn btn-primary">Sign In</button>
                    </div>
                    <div>
                        <Button variant="link" onClick={handleShow}>Forget Password?</Button>
                    </div>
                    <Modal show={show} onHide={handleClose}>
                        <Modal.Header>
                            <Modal.Title>Restore Password</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <input
                                type="email"
                                className="form-control"
                                id="email"
                                name="email"
                                onChange={e => setEmail(e.target.value)}
                                placeholder="example@example.com"
                                required
                            />
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="secondary" onClick={handleClose}>
                                Close
                            </Button>
                            <Button variant="primary" onClick={handleSendEmail}>
                                Send Email
                            </Button>
                        </Modal.Footer>
                    </Modal>

                </div>
            </form>
        </div>
    );
}

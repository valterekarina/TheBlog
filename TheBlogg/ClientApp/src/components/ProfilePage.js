import React, { useEffect, useState } from 'react';
import Modal from 'react-bootstrap/Modal';

export const ProfilePage = ({ role }) => {

    const [existingName, setExistingName] = useState('');
    const [emailOld, setOldEmail] = useState('');

    const [name, setName] = useState(existingName);
    const [email, setEmail] = useState(emailOld);
    const [passwordHash, setPasswordHash] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const [show1, setShow1] = useState(false);

    const handleClose1 = () => setShow1(false);
    const handleShow1 = () => {
        setShow1(true);
        setName(existingName);
        setEmail(emailOld);
    }

    const [show2, setShow2] = useState(false);

    const handleClose2 = () => setShow2(false);
    const handleShow2 = () => setShow2(true);

    useEffect(() => {
        (
            async () => {
                const response = await fetch("http://localhost:5000/api/profile", {
                    headers: { "Content-Type": "application/json" },
                    credentials: 'include',
                });

                const content = await response.json();
                setExistingName(content.name);
                setOldEmail(content.email);
            }
        )();
    });

    const handleDelete = async () => {
        if (window.confirm("Do you really want to delete your profile?") === true) {
            try {
                const response = await fetch("http://localhost:5000/api/delete", {
                    method: "DELETE",
                    headers: { "Content-Type": "application/json" },
                    credentials: 'include',
                    body: JSON.stringify({
                        emailOld
                    }),
                });

                if (response.ok) {
                    window.location.href = "/sign-in"
                } else {
                    const errorData = await response.json();
                    console.log("Error: ", errorData);
                }
            } catch (error) {
                console.error("Error: ", error);
            }
        }
    }

    const handleUpdateProfile = async () => {
        try {
            const response = await fetch("http://localhost:5000/api/update-profile", {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: 'include',
                body: JSON.stringify({
                    name,
                    emailOld,
                    email
                }),
            });

            if (response.ok) {
                handleClose1();
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleUpdatePassword = async () => {
        if (passwordHash === confirmPassword) {
            try {
                const response = await fetch("http://localhost:5000/api/update-password", {
                    method: "PUT",
                    headers: { "Content-Type": "application/json" },
                    credentials: 'include',
                    body: JSON.stringify({
                        emailOld,
                        passwordHash
                    }),
                });

                if (response.ok) {
                    handleClose2();
                } else {
                    const errorData = await response.json();
                    console.log("Error: ", errorData);
                }
            } catch (error) {
                console.error("Error: ", error);
            }

        } else {
            alert("Passwords are not the same");
        }
    }

    return (
        <div>
            {(role === undefined) && (
                <div>
                    <h1>You don't have access for this page</h1>
                </div>
            )}
            {(role === 'admin' || role === 'user') && (
                <div>
                    <h1> Profile</h1>
                    <div className="form-group">
                        <label htmlFor="name">Name: </label>
                        <input
                            type="name"
                            className="form-control"
                            id="name"
                            name="name"
                            value={existingName}
                            disabled
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="email">E-mail: </label>
                        <input
                            type="email"
                            className="form-control"
                            id="email"
                            name="email"
                            value={emailOld}
                            disabled
                        />
                    </div>
                    <div>
                        <button className="btn btn-primary" onClick={handleShow1}>Edit Profile</button> &nbsp;
                        <button className="btn btn-danger" onClick={handleDelete}>Delete Profile</button>
                    </div>
                    <br />
                    <div>
                        <button className="btn btn-primary" onClick={handleShow2}>Change Password</button>
                    </div>

                    <Modal show={show1} onHide={handleClose1}>
                        <Modal.Header>
                            <Modal.Title>Edit Profile</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div>
                                <label htmlFor="name">Name: </label>
                                <input
                                    type="text"
                                    className="form-control"
                                    id="name"
                                    name="name"
                                    value={name}
                                    onChange={e => setName(e.target.value)}
                                    placeholder="John Doe"
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="email">E-mail: </label>
                                <input
                                    type="email"
                                    className="form-control"
                                    id="email"
                                    name="email"
                                    value={email}
                                    onChange={e => setEmail(e.target.value)}
                                    placeholder="example@example.com"
                                    required
                                />
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <button className="btn btn-secondary" onClick={handleClose1}>
                                Close
                            </button>
                            <button className="btn btn-primary" onClick={handleUpdateProfile}>
                                Save Changes
                            </button>
                        </Modal.Footer>
                    </Modal>

                    <Modal show={show2} onHide={handleClose2}>
                        <Modal.Header>
                            <Modal.Title>Change password</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <div className="form-group">
                                <label htmlFor="password">New Password: </label>
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
                            <div className="form-group">
                                <label htmlFor="confirmPassword">Confirm new password: </label>
                                <input
                                    type="password"
                                    className="form-control"
                                    id="confirmPassword"
                                    name="confirmPassword"
                                    onChange={e => setConfirmPassword(e.target.value)}
                                    placeholder="*******"
                                    required
                                />
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <button className="btn btn-secondary" onClick={handleClose2}>
                                Close
                            </button>
                            <button className="btn btn-primary" onClick={handleUpdatePassword}>
                                Save Changes
                            </button>
                        </Modal.Footer>
                    </Modal>
                </div>
            )}
        </div>
    );
}
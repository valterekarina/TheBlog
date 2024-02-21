import React, { useState } from 'react';
import { Redirect } from 'react-router-dom';

export const SignUp = () => {

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [passwordHash, setPasswordHash] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [redirect, setRedirect] = useState(false);

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (passwordHash === confirmPassword) {
            try {
                const response = await fetch("http://localhost:5000/api/register", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        name,
                        email,
                        passwordHash
                    }),
                });

                if (response.ok) {
                    setRedirect(true);
                } else {
                    const errorData = await response.json();
                    console.log("Registration error: ", errorData);
                }
            } catch (error) {
                console.error("Registration error: ", error);
            }
        }
        else {
            alert("Check entered info");
        }
    };

    if (redirect) {
        return <Redirect to="/sign-in" />;
    }

    return (
        <div>
            <h2>Sign up</h2>
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <div>
                        <label htmlFor="name">Name: </label>
                        <input
                            type="text"
                            className="form-control"
                            id="name"
                            name="name"
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
                    <div className="form-group">
                        <label htmlFor="confirmPassword">Confirm password: </label>
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
                    <div>
                        <button type="submit" className="btn btn-primary">Sign Up</button>
                    </div>

                </div>
            </form>
        </div>
    );
}
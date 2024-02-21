import React from 'react';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export const NavMenu = ({ role, setRole }) => {

    const LogOut = async () => {
        await fetch("http://localhost:5000/api/logout", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: 'include',
        });
        setRole(' ');
    }

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                <Container>
                    <NavbarBrand tag={Link} to="/">The Blog</NavbarBrand>
                    {(role === undefined || role === ' ') && (
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/sign-in">Sign In</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/sign-up">Sign Up</NavLink>
                            </NavItem>
                        </ul>
                    )}

                    {role === 'user' && (
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/profile">Profile</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/" onClick={LogOut}>Log Out</NavLink>
                            </NavItem>
                        </ul>
                    )}

                    {role === 'admin' && (
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/users-list">User List</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/profile">Profile</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/" onClick={LogOut}>Log Out</NavLink>
                            </NavItem>
                        </ul>
                    )}

                </Container>
            </Navbar>
        </header>
    );
}

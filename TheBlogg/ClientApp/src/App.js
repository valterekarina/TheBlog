import React, { useEffect, useState } from 'react';
import { BrowserRouter, Route } from 'react-router-dom';
import { Home } from './components/Home';
import { SignUp } from './components/SignUp'
import './custom.css'
import { SignIn } from './components/SignIn';
import { NavMenu } from './components/NavMenu';
import { ProfilePage } from './components/ProfilePage';
import { UsersList } from './components/UsersList';

export const App = () => {

    const [role, setRole] = useState('');
    const [canWrite, setCanWrite] = useState(false);
    const [signedInUserId, setSignedInUserId] = useState(0);
    const [canRankArticle, setCanRankArticle] = useState(false);
    const [canCommentArticle, setCanCommentArticle] = useState(false);


    useEffect(() => {
        (
            async () => {
                const response = await fetch("http://localhost:5000/api/profile", {
                    headers: { "Content-Type": "application/json" },
                    credentials: 'include',
                })

                const content = await response.json();
                setRole(content.role);
                setCanWrite(content.canCreateArticle);
                setCanRankArticle(content.canRank);
                setSignedInUserId(content.id);
                setCanCommentArticle(content.canComment);
            }
        )();
    });

    return (
        <BrowserRouter>
            <NavMenu role={role} setRole={setRole} />
            <Route exact path='/' component={() => <Home canWrite={canWrite} signedInUserId={signedInUserId} role={role}
                canRankArticle={canRankArticle} canCommentArticle={canCommentArticle} />} />
            <Route path='/sign-in' component={() => <SignIn setRole={setRole} />} />
            <Route path='/sign-up' component={SignUp} />
            <Route path='/profile' component={() => <ProfilePage role={role} />} />
            <Route path='/users-list' component={() => <UsersList role={role} />} />
        </BrowserRouter>
    );
}

import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Accordion from 'react-bootstrap/Accordion';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import './Home.css'



export const Home = ({ canWrite, signedInUserId, role, canRankArticle, canCommentArticle }) => {

    const initialFieldValues = {
        title: '',
        description: '',
        imageUrl: '',
        userId: signedInUserId,
        imageSrc: '/DefaultImage.png',
        imageFile: null,
    }

    const [formData, setFormData] = useState(initialFieldValues);

    const [articleList, setArticleList] = useState([]);
    const [commentId, setCommentId] = useState(0);
    const [commentText, setCommentText] = useState("");

    const [searchQuery, setSearchQuery] = useState("");

    const [topArticlesToShow, setTopArticlesToShow] = useState([]);
    const [commentArticlesToShow, setCommentArticlesToShow] = useState([]);
    const [searchedArticles, setSearchedArticles] = useState([]);

    const [show1, setShow1] = useState(false);
    const [show2, setShow2] = useState(false);
    const [show3, setShow3] = useState(false);

    const [showArticleList, setShowArticleList] = useState(true);
    const [showTopArticles, setShowTopArticles] = useState(false);
    const [showLastCommentedArticles, setShowLastCommentedArticles] = useState(false);
    const [showSearchedArticles, setShowSearchedArticles] = useState(false);

    const handleClose1 = () => setShow1(false);
    const handleShow1 = () => setShow1(true);

    const handleClose2 = () => setShow2(false);
    const handleShow2 = () => setShow2(true);

    const handleClose3 = () => setShow3(false);
    const handleShow3 = () => setShow3(true);

    useEffect(() => {
        (
            async () => {
                const response = await fetch("http://localhost:5000/api/aricle-list");
                if (!response.ok) {
                    console.log("error");
                }

                const content = await response.json();
                setArticleList(content);
            }
        )();
    }, [articleList]);

    const handleFileChange = (e) => {
        const file = e.target.files[0];

        if (file) {
            const reader = new FileReader();
            reader.onload = (event) => {
                setFormData({
                    ...formData,
                    imageSrc: event.target.result,
                    imageFile: file
                });
            }
            reader.readAsDataURL(file);
        } else {
            setFormData({
                ...formData,
                imageSrc: '/DefaultImage.png',
                imageFile: null
            });
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleAddArticle = async (e) => {
        e.preventDefault();

        const form = new FormData();
        form.append('title', formData.title);
        form.append('description', formData.description);
        form.append('imageUrl', formData.imageUrl);
        form.append('userId', formData.userId);
        form.append('imageFile', formData.imageFile);

        try {
            const response = await fetch("http://localhost:5000/api/create-article", {
                method: 'POST',
                body: form,
            });

            if (response.ok) {
                const result = await response.json();
                console.log('Article created: ', result);
                handleClose1();
                setFormData(initialFieldValues);
            } else {
                console.log("failed to create article", response.statusText)
            }

        } catch {
            console.log("error");
        }
    };

    const cancelAddArticle = () => {
        handleClose1();
        setFormData(initialFieldValues);
    }

    const handleEdit = async (article) => {
        await setFormData(article);
        handleShow2();
    }

    const cancelEditArticle = () => {
        handleClose2();
        setFormData(initialFieldValues);
    }

    const handleUpdateArticle = async (e) => {
        e.preventDefault();
        const form = new FormData();
        form.append('id', formData.id);
        form.append('title', formData.title);
        form.append('description', formData.description);
        form.append('imageUrl', formData.imageUrl);
        form.append('userId', formData.userId);
        form.append('imageFile', formData.imageFile);

        try {
            const response = await fetch("http://localhost:5000/api/update-article", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                handleClose2();
                setFormData(initialFieldValues);
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleDelete = async (article) => {
        if (window.confirm('Do you really want to delete \'' + article.title + '\'?')) {

            const form = new FormData();
            form.append('id', article.id);
            form.append('title', article.title);
            form.append('description', article.description);
            form.append('imageUrl', article.imageUrl);
            form.append('userId', article.userId);
            form.append('imageFile', article.imageFile);


            try {
                const response = await fetch("http://localhost:5000/api/delete-article", {
                    method: "DELETE",
                    body: form
                });

                if (response.ok) {
                    console.log("article deleted")
                } else {
                    const errorData = await response.json();
                    console.log("Error: ", errorData);
                }
            } catch (error) {
                console.error("Error: ", error);
            }
        }
    }

    const UpdateRatingUp = async (article) => {

        const response = await fetch("http://localhost:5000/api/get-user-votes");
        if (!response.ok) {
            console.log("error");
        }

        const content = await response.json();
        const userVotesAll = await content.filter(uv => uv.userId === signedInUserId);
        const userVotesForArticle = await userVotesAll.filter(v => v.articleId === article.id)

        const voteForm = new FormData();

        if (userVotesForArticle.length !== 0) {
            if (userVotesForArticle[0].vote === 1) {
                alert("You already voted for this article +1");
                return;
            } else {
                voteForm.append('id', userVotesForArticle[0].id);
            }
        }

        voteForm.append('userId', signedInUserId);
        voteForm.append('articleId', article.id);
        voteForm.append('vote', 1);

        const form = new FormData();
        form.append('id', article.id);
        form.append('rating', article.rating + 1);

        UpdateUserVote(voteForm)
        UpdateRating(form)
    }

    const UpdateRatingDown = async (article) => {

        const response = await fetch("http://localhost:5000/api/get-user-votes");
        if (!response.ok) {
            console.log("error");
        }

        const content = await response.json();
        const userVotesAll = await content.filter(uv => uv.userId === signedInUserId);
        const userVotesForArticle = await userVotesAll.filter(v => v.articleId === article.id)

        const voteForm = new FormData();

        if (userVotesForArticle.length !== 0) {
            if (userVotesForArticle[0].vote === -1) {
                alert("You already voted for this article -1");
                return;
            } else {
                voteForm.append('id', userVotesForArticle[0].id);
            }
        }

        voteForm.append('userId', signedInUserId);
        voteForm.append('articleId', article.id);
        voteForm.append('vote', -1);

        const form = new FormData();
        form.append('id', article.id);
        form.append('rating', article.rating - 1);

        UpdateUserVote(voteForm)
        UpdateRating(form)
    }

    const UpdateUserVote = async (voteForm) => {
        try {
            const response = await fetch("http://localhost:5000/api/update-userVote", {
                method: "PUT",
                body: voteForm
            });

            if (response.ok) {

            } else {
                const errorData = await response.json();
                console.log("Error1: ", errorData);
            }
        } catch (error) {
            console.error("Error2: ", error);
        }
    }

    const UpdateRating = async (form) => {

        try {
            const response = await fetch("http://localhost:5000/api/update-rating", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                hadleGetTopArticles();
                handleGetLatedtCommentedArticles();
            } else {
                const errorData = await response.json();
                console.log("Error1: ", errorData);
            }
        } catch (error) {
            console.error("Error2: ", error);
        }
    }

    const handleComment = async (articleId) => {
        const form = new FormData();
        form.append('text', commentText);
        form.append('userId', signedInUserId);
        form.append('articleId', articleId);

        try {
            const response = await fetch("http://localhost:5000/api/add-comment", {
                method: 'POST',
                body: form,
            });

            if (response.ok) {
                setCommentText("");
                const result = await response.json();
                console.log('Comment created: ', result);

            } else {
                console.log("failed to create article", response.statusText)
            }

        } catch {
            console.log("error");
        }
    }

    const handleEditComment = async (comment) => {
        await setCommentId(comment.id);
        await setCommentText(comment.text);
        handleShow3();
    }

    const cancelEditComment = () => {
        handleClose3();
        setCommentText("");
    }

    const handleUpdateComment = async (e) => {
        e.preventDefault();
        const form = new FormData();
        form.append('id', commentId);
        form.append('text', commentText);

        try {
            const response = await fetch("http://localhost:5000/api/update-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                handleClose3();
                setCommentText("");
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleDeleteComment = async (comment) => {
        if (window.confirm('Do you really want to delete this comment?')) {

            const form = new FormData();
            form.append('id', comment.id);

            try {
                const response = await fetch("http://localhost:5000/api/delete-comment", {
                    method: "DELETE",
                    body: form
                });

                if (response.ok) {
                    console.log("comment deleted")
                } else {
                    const errorData = await response.json();
                    console.log("Error: ", errorData);
                }
            } catch (error) {
                console.error("Error: ", error);
            }
        }
    }

    const handleShowArticles = () => {
        setShowArticleList(true);
        setShowLastCommentedArticles(false);
        setShowTopArticles(false);
        setShowSearchedArticles(false);
    }

    const hadleGetTopArticles = () => {
        const sortedArticles = articleList.sort((a, b) => b.rating - a.rating);
        const topThreeArticles = sortedArticles.slice(0, 3);
        setTopArticlesToShow(topThreeArticles);
    }

    const handleShowTopArticles = () => {
        hadleGetTopArticles();

        setShowArticleList(false);
        setShowLastCommentedArticles(false);
        setShowTopArticles(true);
        setShowSearchedArticles(false);
    }

    const handleGetLatedtCommentedArticles = () => {
        const articlesWithLatestComment = articleList.map(article => {
            const latestComment = article.comments.length > 0
                ? article.comments.reduce((latest, current) => (new Date(current.createdAt) > new Date(latest.createdAt) ? current : latest))
                : null;

            return {
                ...article,
                latestComment: latestComment
            };
        });
        const sortedArticles = [...articlesWithLatestComment].sort((a, b) => {
            const aTimestamp = a.latestComment ? new Date(a.latestComment.createdAt).getTime() : 0;
            const bTimestamp = b.latestComment ? new Date(b.latestComment.createdAt).getTime() : 0;

            return bTimestamp - aTimestamp;
        });

        const topThreeArticles = sortedArticles.slice(0, 3);

        setCommentArticlesToShow(topThreeArticles);
    }

    const handleLastCommentedArticles = () => {
        handleGetLatedtCommentedArticles();
        setShowArticleList(false);
        setShowLastCommentedArticles(true);
        setShowTopArticles(false);
        setShowSearchedArticles(false);
    }

    const handleSearch = () => {
        const filteredArticles = articleList.filter(article =>
            article.title.toLowerCase().includes(searchQuery.toLowerCase())
        );

        setSearchedArticles(filteredArticles);
        setSearchQuery("");

        setShowArticleList(false);
        setShowLastCommentedArticles(false);
        setShowTopArticles(false);
        setShowSearchedArticles(true);
    }

    const handleBlockComment = async (comment) => {
        const form = new FormData();
        form.append('id', comment.id);
        form.append('isBlocked', true);
        form.append('isReported', comment.isReported);

        try {
            const response = await fetch("http://localhost:5000/api/block-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                console.log("comment blocked")
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleUnblockComment = async (comment) => {
        const form = new FormData();
        form.append('id', comment.id);
        form.append('isBlocked', false);
        form.append('isReported', false);

        try {
            const response = await fetch("http://localhost:5000/api/block-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                console.log("comment unblocked")
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleReportComment = async (comment) => {
        const form = new FormData();
        form.append('id', comment.id);
        form.append('isReported', true);
        form.append('isComplained', comment.isComplained);

        try {
            const response = await fetch("http://localhost:5000/api/report-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                console.log("comment reported")
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleUnreportComment = async (comment) => {
        const form = new FormData();
        form.append('id', comment.id);
        form.append('isReported', false);
        form.append('isComplained', false);

        try {
            const response = await fetch("http://localhost:5000/api/report-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                console.log("comment unreported")
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    const handleComplainComment = async (comment) => {
        const form = new FormData();
        form.append('id', comment.id);
        form.append('isComplained', true);

        try {
            const response = await fetch("http://localhost:5000/api/complain-comment", {
                method: "PUT",
                body: form
            });

            if (response.ok) {
                console.log("comment complained")
            } else {
                const errorData = await response.json();
                console.log("Error: ", errorData);
            }
        } catch (error) {
            console.error("Error: ", error);
        }
    }

    return (
        <div className="homeDiv">
            <div>
                <input type="text" value={searchQuery} onChange={(e) => setSearchQuery(e.target.value)} placeholder="search..." /> &nbsp;
                <Button variant="primary" onClick={handleSearch}>Search</Button>
            </div>
            <Nav className="justify-content-center" variant="tabs" defaultActiveKey="/">
                <Nav.Item>
                    <Button variant='link' onClick={handleShowArticles}>Articles</Button>
                </Nav.Item>
                <Nav.Item>
                    <Button variant='link' onClick={handleShowTopArticles}>Top 3</Button>
                </Nav.Item>
                <Nav.Item>
                    <Button variant='link' onClick={handleLastCommentedArticles}>Last Commented</Button>
                </Nav.Item>
            </Nav>
            {(canWrite && showArticleList) && (
                <div className="forAddButton">
                    <Button variant="primary" className="addArticleButton" onClick={handleShow1}>Add Article</Button>
                </div>
            )}
            <div className="forAccordion">
                {showArticleList && (
                    <Accordion>
                        {articleList.slice(-5).toReversed().map((article, index) => (
                            <div>
                                <Accordion.Item eventKey={index + 1}>
                                    <Accordion.Header>
                                        <div className="forAccordionHeader">
                                            <img src={article.imageSrc} className="accordionImage" alt={article.title} />
                                            {article.title}
                                        </div>
                                    </Accordion.Header>
                                    <Accordion.Body>
                                        {article.description}
                                        <div className="ranking">
                                            Rating:&nbsp;
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => { UpdateRatingDown(article) }}>-</Button>
                                            )}
                                            {article.rating}
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => UpdateRatingUp(article)}>+</Button>
                                            )}
                                        </div>

                                        {(role === 'admin' || article.userId === signedInUserId) && (
                                            <div className="forAddButton">
                                                <Button variant="danger" className="editDeleteButton" onClick={() => handleDelete(article)}>Delete</Button>&nbsp;
                                                <Button variant="primary" className="editDeleteButton" onClick={() => handleEdit(article)}>Edit</Button>
                                            </div>
                                        )}
                                        <div className="for-comments">
                                            <h4>
                                                Comments:
                                            </h4>
                                            <div >
                                                {article.comments && article.comments.map((comment, commentIndex) => (
                                                    <div>
                                                        {(!comment.isBlocked || role === "admin") && (
                                                            <div className="comments" key={commentIndex}>
                                                                <div className="comment-datetime">
                                                                    {new Date(comment.createdAt).toLocaleString()}
                                                                </div>
                                                                <div>
                                                                    {comment.isBlocked && (
                                                                        <div className="blocked-comment">Blocked!</div>
                                                                    )}
                                                                    {(comment.isReported && !comment.isBlocked && !comment.isComplained) && (
                                                                        <div className="reported-comment">Reported!</div>
                                                                    )}
                                                                    {(comment.isReported && comment.isComplained && !comment.isBlocked) && (
                                                                        <div className="reported-comment">Reported and Complained!</div>
                                                                    )}
                                                                    {comment.text}
                                                                </div>
                                                                <div className="comment-editdelete-button">
                                                                    {((role === 'admin' || role === 'user') && !comment.isReported && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleReportComment(comment)}>Report</Button>
                                                                    )}
                                                                    {(comment.isReported && !comment.isComplained && (role === 'admin' || role === 'user') && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleComplainComment(comment)}>Request to unreport</Button>
                                                                    )}
                                                                    {(role === 'admin' || (comment.userId === signedInUserId && canCommentArticle)) && (
                                                                        <div >
                                                                            <Button variant="link" className="editDeleteButton-comment" onClick={() => handleEditComment(comment)}>Edit</Button>
                                                                            <Button variant="link" className="editDeleteButton-comment" onClick={() => handleDeleteComment(comment)}>Delete</Button>
                                                                            {comment.isBlocked && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnblockComment(comment)}>Unblock</Button>
                                                                            )}
                                                                            {(comment.isReported && !comment.isBlocked && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleBlockComment(comment)}>Block</Button>
                                                                            )}
                                                                            {(comment.isReported && comment.isComplained && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnreportComment(comment)}>Unreport</Button>
                                                                            )}
                                                                            
                                                                        </div>
                                                                    )}
                                                                </div>
                                                                
                                                            </div>
                                                        )}
                                                    </div>
                                                ))}
                                            </div>
                                            <div>
                                                {canCommentArticle && (
                                                    <form>
                                                        <textarea className="form-control" id="commenttext" name="commenttext" placeholder="Comment"
                                                            value={commentText} onChange={e => setCommentText(e.target.value)} rows="4" />
                                                        <Button variant="primary" className="add-comment-button" onClick={() => handleComment(article.id)} disabled={!commentText}>Comment</Button>
                                                    </form>
                                                )}
                                            </div>
                                        </div>
                                    </Accordion.Body>
                                </Accordion.Item>
                                <br />
                            </div>
                        ))}
                    </Accordion>
                )}

                {showSearchedArticles && (
                    <Accordion>
                        {searchedArticles.map((article, index) => (
                            <div>
                                <Accordion.Item eventKey={index + 1}>
                                    <Accordion.Header>
                                        <div className="forAccordionHeader">
                                            <img src={article.imageSrc} className="accordionImage" alt={article.title} />
                                            {article.title}
                                        </div>
                                    </Accordion.Header>
                                    <Accordion.Body>
                                        {article.description}
                                        <div className="ranking">
                                            Rating:&nbsp;
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => { UpdateRatingDown(article) }}>-</Button>
                                            )}
                                            {article.rating}
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => UpdateRatingUp(article)}>+</Button>
                                            )}
                                        </div>

                                        {(role === 'admin' || article.userId === signedInUserId) && (
                                            <div className="forAddButton">
                                                <Button variant="danger" className="editDeleteButton" onClick={() => handleDelete(article)}>Delete</Button>&nbsp;
                                                <Button variant="primary" className="editDeleteButton" onClick={() => handleEdit(article)}>Edit</Button>
                                            </div>
                                        )}
                                        <div className="for-comments">
                                            <h4>
                                                Comments:
                                            </h4>
                                            <div >
                                                {article.comments && article.comments.map((comment, commentIndex) => (
                                                    <div>
                                                        {(!comment.isBlocked || role === "admin") && (
                                                            <div className="comments" key={commentIndex}>
                                                                <div className="comment-datetime">
                                                                    {new Date(comment.createdAt).toLocaleString()}
                                                                </div>
                                                                <div>
                                                                    {comment.isBlocked && (
                                                                        <div className="blocked-comment">Blocked!</div>
                                                                    )}
                                                                    {(comment.isReported && !comment.isBlocked && !comment.isComplained) && (
                                                                        <div className="reported-comment">Reported!</div>
                                                                    )}
                                                                    {(comment.isReported && comment.isComplained && !comment.isBlocked) && (
                                                                        <div className="reported-comment">Reported and Complained!</div>
                                                                    )}
                                                                    {comment.text}
                                                                </div>
                                                                <div className="comment-editdelete-button">
                                                                    {((role === 'admin' || role === 'user') && !comment.isReported && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleReportComment(comment)}>Report</Button>
                                                                    )}
                                                                    {(comment.isReported && !comment.isComplained && (role === 'admin' || role === 'user') && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleComplainComment(comment)}>Request to unreport</Button>
                                                                    )}
                                                                    {(role === 'admin' || (comment.userId === signedInUserId && canCommentArticle)) && (
                                                                        <div >
                                                                            <Button variant="link" className="editDeleteButton-comment" onClick={() => handleEditComment(comment)}>Edit</Button>
                                                                            <Button variant="link" className="editDeleteButton-comment" onClick={() => handleDeleteComment(comment)}>Delete</Button>
                                                                            {comment.isBlocked && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnblockComment(comment)}>Unblock</Button>
                                                                            )}
                                                                            {(comment.isReported && !comment.isBlocked && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleBlockComment(comment)}>Block</Button>
                                                                            )}
                                                                            {(comment.isReported && comment.isComplained && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnreportComment(comment)}>Unreport</Button>
                                                                            )}
                                                                            
                                                                        </div>
                                                                    )}
                                                                </div>
                                                                
                                                            </div>
                                                        )}
                                                    </div>
                                                ))}
                                            </div>
                                            <div>
                                                {canCommentArticle && (
                                                    <form>
                                                        <textarea className="form-control" id="commenttext" name="commenttext" placeholder="Comment"
                                                            value={commentText} onChange={e => setCommentText(e.target.value)} rows="4" />
                                                        <Button variant="primary" className="add-comment-button" onClick={() => handleComment(article.id)} disabled={!commentText}>Comment</Button>
                                                    </form>
                                                )}
                                            </div>
                                        </div>
                                    </Accordion.Body>
                                </Accordion.Item>
                                <br />
                            </div>
                        ))}
                    </Accordion>
                )}

                {showTopArticles && (
                    <Accordion>
                        {topArticlesToShow.map((articles, index) => (
                            <div>
                                <Accordion.Item eventKey={index + 1}>
                                    <Accordion.Header>
                                        <div className="forAccordionHeader">
                                            <img src={articles.imageSrc} className="accordionImage" alt={articles.title} />
                                            {articles.title}
                                        </div>
                                    </Accordion.Header>
                                    <Accordion.Body>
                                        {articles.description}
                                        <div className="ranking">
                                            Rating:&nbsp;
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => { UpdateRatingDown(articles) }}>-</Button>
                                            )}
                                            {articles.rating}
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => UpdateRatingUp(articles)}>+</Button>
                                            )}
                                        </div>

                                        {/*{(role === 'admin' || article.userId === signedInUserId) && (*/}
                                        {/*    <div className="forAddButton">*/}
                                        {/*        <Button variant="danger" className="editDeleteButton" onClick={() => handleDelete(article)}>Delete</Button>&nbsp;*/}
                                        {/*        <Button variant="primary" className="editDeleteButton" onClick={() => handleEdit(article)}>Edit</Button>*/}
                                        {/*    </div>*/}
                                        {/*)}*/}

                                        <div className="for-comments">
                                            <h4>
                                                Comments:
                                            </h4>
                                            <div >
                                                {articles.comments && articles.comments.map((comment, commentIndex) => (
                                                    <div>
                                                        {(!comment.isBlocked || role === "admin") && (
                                                            <div className="comments" key={commentIndex}>
                                                                <div className="comment-datetime">
                                                                    {new Date(comment.createdAt).toLocaleString()}
                                                                </div>
                                                                <div>
                                                                    {comment.isBlocked && (
                                                                        <div className="blocked-comment">Blocked!</div>
                                                                    )}
                                                                    {(comment.isReported && !comment.isBlocked && !comment.isComplained) && (
                                                                        <div className="reported-comment">Reported!</div>
                                                                    )}
                                                                    {(comment.isReported && comment.isComplained && !comment.isBlocked) && (
                                                                        <div className="reported-comment">Reported and Complained!</div>
                                                                    )}
                                                                    {comment.text}
                                                                </div>
                                                                <div className="comment-editdelete-button">
                                                                    {((role === 'admin' || role === 'user') && !comment.isReported && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleReportComment(comment)}>Report</Button>
                                                                    )}
                                                                    {(comment.isReported && !comment.isComplained && (role === 'admin' || role === 'user') && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleComplainComment(comment)}>Request to unreport</Button>
                                                                    )}
                                                                    {(role === 'admin' || (comment.userId === signedInUserId && canCommentArticle)) && (
                                                                        <div >
                                                                            {/*<Button variant="link" className="editDeleteButton-comment" onClick={() => handleEditComment(comment)}>Edit</Button>*/}
                                                                            {/*<Button variant="link" className="editDeleteButton-comment" onClick={() => handleDeleteComment(comment)}>Delete</Button>*/}
                                                                            {comment.isBlocked && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnblockComment(comment)}>Unblock</Button>
                                                                            )}
                                                                            {(comment.isReported && !comment.isBlocked && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleBlockComment(comment)}>Block</Button>
                                                                            )}
                                                                            {(comment.isReported && comment.isComplained && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnreportComment(comment)}>Unreport</Button>
                                                                            )}

                                                                        </div>
                                                                    )}
                                                                </div>

                                                            </div>
                                                        )}
                                                    </div>
                                                ))}
                                            </div>


                                            {/*<div>*/}
                                            {/*    {canCommentArticle && (*/}
                                            {/*        <form>*/}
                                            {/*            <textarea className="form-control" id="commenttext" name="commenttext" placeholder="Comment"*/}
                                            {/*                value={commentText} onChange={e => setCommentText(e.target.value)} rows="4" />*/}
                                            {/*            <Button variant="primary" className="add-comment-button" onClick={() => handleComment(article.id)} disabled={!commentText}>Comment</Button>*/}
                                            {/*        </form>*/}
                                            {/*    )}*/}
                                            {/*</div>*/}
                                        </div>

                                    </Accordion.Body>
                                </Accordion.Item>
                                <br />
                            </div>
                        ))}
                    </Accordion>
                )}

                {showLastCommentedArticles && (
                    <Accordion>
                        {commentArticlesToShow.map((articles, index) => (
                            <div>
                                <Accordion.Item eventKey={index + 1}>
                                    <Accordion.Header>
                                        <div className="forAccordionHeader">
                                            <img src={articles.imageSrc} className="accordionImage" alt={articles.title} />
                                            {articles.title}
                                        </div>
                                    </Accordion.Header>
                                    <Accordion.Body>
                                        {articles.description}
                                        <div className="ranking">
                                            Rating:&nbsp;
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => { UpdateRatingDown(articles) }}>-</Button>
                                            )}
                                            {articles.rating}
                                            {canRankArticle && (
                                                <Button variant="outline-secondary" className="rankButtons" onClick={() => UpdateRatingUp(articles)}>+</Button>
                                            )}
                                        </div>

                                        {/*{(role === 'admin' || article.userId === signedInUserId) && (*/}
                                        {/*    <div className="forAddButton">*/}
                                        {/*        <Button variant="danger" className="editDeleteButton" onClick={() => handleDelete(article)}>Delete</Button>&nbsp;*/}
                                        {/*        <Button variant="primary" className="editDeleteButton" onClick={() => handleEdit(article)}>Edit</Button>*/}
                                        {/*    </div>*/}
                                        {/*)}*/}
                                        <div className="for-comments">
                                            <h4>
                                                Comments:
                                            </h4>
                                            <div >
                                                {articles.comments && articles.comments.map((comment, commentIndex) => (
                                                    <div>
                                                        {(!comment.isBlocked || role === "admin") && (
                                                            <div className="comments" key={commentIndex}>
                                                                <div className="comment-datetime">
                                                                    {new Date(comment.createdAt).toLocaleString()}
                                                                </div>
                                                                <div>
                                                                    {comment.isBlocked && (
                                                                        <div className="blocked-comment">Blocked!</div>
                                                                    )}
                                                                    {(comment.isReported && !comment.isBlocked && !comment.isComplained) && (
                                                                        <div className="reported-comment">Reported!</div>
                                                                    )}
                                                                    {(comment.isReported && comment.isComplained && !comment.isBlocked) && (
                                                                        <div className="reported-comment">Reported and Complained!</div>
                                                                    )}
                                                                    {comment.text}
                                                                </div>
                                                                <div className="comment-editdelete-button">
                                                                    {((role === 'admin' || role === 'user') && !comment.isReported && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleReportComment(comment)}>Report</Button>
                                                                    )}
                                                                    {(comment.isReported && !comment.isComplained && (role === 'admin' || role === 'user') && !comment.isBlocked) && (
                                                                        <Button variant="link" className="editDeleteButton-comment" onClick={() => handleComplainComment(comment)}>Request to unreport</Button>
                                                                    )}
                                                                    {(role === 'admin' || (comment.userId === signedInUserId && canCommentArticle)) && (
                                                                        <div >
                                                                            {/*<Button variant="link" className="editDeleteButton-comment" onClick={() => handleEditComment(comment)}>Edit</Button>*/}
                                                                            {/*<Button variant="link" className="editDeleteButton-comment" onClick={() => handleDeleteComment(comment)}>Delete</Button>*/}
                                                                            {comment.isBlocked && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnblockComment(comment)}>Unblock</Button>
                                                                            )}
                                                                            {(comment.isReported && !comment.isBlocked && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleBlockComment(comment)}>Block</Button>
                                                                            )}
                                                                            {(comment.isReported && comment.isComplained && role === 'admin') && (
                                                                                <Button variant="link" className="editDeleteButton-comment" onClick={() => handleUnreportComment(comment)}>Unreport</Button>
                                                                            )}

                                                                        </div>
                                                                    )}
                                                                </div>

                                                            </div>
                                                        )}
                                                    </div>
                                                ))}
                                            </div>


                                            {/*<div>*/}
                                            {/*    {canCommentArticle && (*/}
                                            {/*        <form>*/}
                                            {/*            <textarea className="form-control" id="commenttext" name="commenttext" placeholder="Comment"*/}
                                            {/*                value={commentText} onChange={e => setCommentText(e.target.value)} rows="4" />*/}
                                            {/*            <Button variant="primary" className="add-comment-button" onClick={() => handleComment(article.id)} disabled={!commentText}>Comment</Button>*/}
                                            {/*        </form>*/}
                                            {/*    )}*/}
                                            {/*</div>*/}
                                        </div>
                                    </Accordion.Body>
                                </Accordion.Item>
                                <br />
                            </div>
                        ))}
                    </Accordion>
                )}

            </div>

            <Modal show={show1} onHide={handleClose1}>
                <Modal.Header>
                    <Modal.Title>Add Article</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                            <Form.Label>Article Title</Form.Label>
                            <input type="text" className="form-control" id="title" name="title" value={formData.title} onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <Form.Label>Text</Form.Label>
                            <textarea className="form-control" id="description" name="description" value={formData.description} onChange={handleChange} rows="4" required />
                        </Form.Group>
                        <img className="imagePreview" src={formData.imageSrc} alt="preview" />
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <input type="file" className="form-control" id="imageFile" name="imageFile" accept="image/*" onChange={handleFileChange} required />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={cancelAddArticle}>
                        Close
                    </Button>
                    <Button variant="primary" disabled={formData.imageFile === null} onClick={handleAddArticle}>
                        Add Article
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={show2} onHide={handleClose2}>
                <Modal.Header>
                    <Modal.Title>Edit Article</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                            <Form.Label>Article Title</Form.Label>
                            <input type="text" className="form-control" id="title" name="title" value={formData.title} onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <Form.Label>Text</Form.Label>
                            <textarea className="form-control" id="description" name="description" value={formData.description} onChange={handleChange} rows="4" required />
                        </Form.Group>
                        <img className="imagePreview" src={formData.imageSrc} alt="preview" />
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <input type="file" className="form-control" id="imageFile" name="imageFile" accept="image/*" onChange={handleFileChange} required />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={cancelEditArticle}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleUpdateArticle}>
                        Update Article
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal show={show3} onHide={handleClose3}>
                <Modal.Header>
                    <Modal.Title>Edit Comment</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
                            <Form.Label>Comment</Form.Label>
                            <textarea className="form-control" id="description" name="description" value={commentText} onChange={e => setCommentText(e.target.value)} rows="4" />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={cancelEditComment}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleUpdateComment}>
                        Update Comment
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}

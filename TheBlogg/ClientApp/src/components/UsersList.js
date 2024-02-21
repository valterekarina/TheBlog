import React, { useEffect, useState } from 'react';
import Table from 'react-bootstrap/Table';
import Modal from 'react-bootstrap/Modal';

export const UsersList = ({ role }) => {

    const [rawData, setRawData] = useState([]);
    const [data, setData] = useState([]);
    const [show, setShow] = useState(false);

    const [email, setEmail] = useState('');
    const [canCreateArticle, setcanCreateArticle] = useState(false);
    const [canComment, setcanComment] = useState(false);
    const [canRank, setcanRank] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    useEffect(() => {
        (
            async () => {
                const response = await fetch("http://localhost:5000/api/get-users");
                if (!response.ok) {
                    console.log("error");
                }

                const content = await response.json();
                setRawData(content);
                const usersWithUserRole = await rawData.filter(user => user.role === 'user');
                setData(usersWithUserRole);

            }
        )();
    }, [rawData]);

    const changePermissions = async (eemail, canCreate, canCom, canRa) => {
        setEmail(eemail);
        setcanCreateArticle(canCreate);
        setcanComment(canCom);
        setcanRank(canRa);
        handleShow();
    }

    const handleUpdate = async () => {
        try {
            const response = await fetch("http://localhost:5000/api/update-permissions", {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: 'include',
                body: JSON.stringify({
                    email,
                    canCreateArticle,
                    canComment,
                    canRank
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
            {role === 'admin' && (
                <div>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Create Article</th>
                                <th>Comment</th>
                                <th>Rank</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {data.map((item, index) => (
                                <tr key={item.id}>
                                    <td>{index + 1}</td>
                                    <td>{item.name}</td>
                                    <td>{item.email}</td>
                                    <td>{item.canCreateArticle ? 'yes' : 'no'}</td>
                                    <td>{item.canComment ? 'yes' : 'no'}</td>
                                    <td>{item.canRank ? 'yes' : 'no'}</td>
                                    <td>
                                        <button className="btn btn-primary" onClick={() => changePermissions(item.email, item.canCreateArticle, item.canComment, item.canRank)}>Change permissions</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                    <Modal show={show} onHide={handleClose}>
                        <Modal.Header>
                            <Modal.Title>Change permisions</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <h4>{email}</h4>
                            <div>
                                <input type="checkbox" checked={canCreateArticle} onChange={() => setcanCreateArticle(!canCreateArticle)} value={canCreateArticle} /><label>&nbsp;Can create articles</label>
                            </div>
                            <div>
                                <input type="checkbox" checked={canComment} onChange={() => setcanComment(!canComment)} value={canComment} /><label>&nbsp;Can comment</label>
                            </div>
                            <div>
                                <input type="checkbox" checked={canRank} onChange={() => setcanRank(!canRank)} value={canRank} /><label>&nbsp;Can rank</label>
                            </div>
                        </Modal.Body>
                        <Modal.Footer>
                            <button className="btn btn-secondary" onClick={handleClose}>
                                Close
                            </button>
                            <button className="btn btn-primary" onClick={handleUpdate}>
                                Save Changes
                            </button>
                        </Modal.Footer>
                    </Modal>
                </div>
            )}
        </div>

    );
}
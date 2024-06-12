import React, { useState } from 'react';
import { Table, Dropdown, Form, Button, Container, Modal } from 'react-bootstrap';
import { Role } from './ProfileStoreNav';
import { RiLockLine } from 'react-icons/ri'; // Importing lock icon
import { getToken } from '../services/SessionService';

// Enum for possible permissions
enum Permission {
    addProduct = 'Add Product',
    removeProduct = 'Remove Product',
    updateProductPrice = 'Update Product Price',
    updateProductDiscount = 'Update Product Discount',
    updateProductQuantity = 'Update Product Quantity',
    editPermissions = 'Edit Permissions',
}

interface TableRowProps {
    role: Role;
    index: number;
}

const TableRow: React.FC<TableRowProps> = ({ role, index }) => {
    const isOwner = role.role === 'Owner';
    const isFounder = role.role === 'Founder';

    return (
        <tr>
            <td>{index + 1}</td> 
            <td>{role.username}</td>
            <td>{role.role}</td>
            <td>{role.appointer}</td>
            <td>
                {isOwner && (
                   <div className="text-success">
                       Have full permission except close Store 
                       <RiLockLine size={30} /> {/* RiLockLine icon */}
                       <i className="fas fa-lock ml-2"></i> {/* Lock icon */}
                   </div>
                )}
                {isFounder && (
                    <div className="text-success">
                        Have full permission
                        <RiLockLine size={30} /> {/* RiLockLine icon */}
                        <i className="fas fa-lock ml-2"></i> {/* Lock icon */}
                    </div>
                )}
                {!isOwner && !isFounder && (
                    <Dropdown>
                        <Dropdown.Toggle variant="success" id="dropdown-basic">
                            Permissions
                        </Dropdown.Toggle>
                        <Dropdown.Menu>
                            <Dropdown.Item>
                                <Form>
                                    {Object.keys(Permission).map((permission: string) => (
                                        <div key={`permission-${permission}`} className="mb-3">
                                            <Form.Check
                                                type="checkbox"
                                                id={`permission-checkbox-${permission}`}
                                                label={(Permission as Record<string, string>)[permission]}
                                                checked={role.permissions.includes(permission)}
                                            />
                                        </div>
                                    ))}
                                </Form>
                            </Dropdown.Item>
                        </Dropdown.Menu>
                    </Dropdown>
                )}
            </td>
        </tr>
    );
};

interface MyTableProps {
    roles: Role[];
    storeId : number;
}

const MyTable: React.FC<MyTableProps> = ({ roles , storeId}) => {
    const [showAppointeeModal, setShowAppointeeModal] = useState(false);
    const [memberUserName, setName] = useState('');
    const [roleName, setRole] = useState('');
    const [appointer, setAppionter] = useState('');
    const [permission, setPermission] = useState<string[]>([]);
    
    const handleAddAppointeeClick = () => {
        setShowAppointeeModal(true);
    };

    const handleCloseAppointeeModal = () => {
        setShowAppointeeModal(false);
    };

    const handleAddAppointeeSubmit = () => {
        fetch(`https://localhost:7163/api/Market/Store/${storeId}/Staff?identifier=${getToken()}`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
            body: JSON.stringify({memberUserName ,permission, roleName}),
        }).then((r) => {
            if (r.ok) {
                return r.json();
            } else {
                throw new Error('Failed to add appointee');
            }
        }).then((data) => {
            console.log('Appointee added successfully:', data);
            handleCloseAppointeeModal();
        }
        ).catch((error) => {
            console.error('Error adding appointee:', error);
        });
    };


    return (
        <>
            <Container className="d-flex justify-content-end my-3">
                <Button variant="primary" onClick={handleAddAppointeeClick}>Add Appointee</Button>
            </Container>
            <Table striped bordered hover className="my-3">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Name</th>
                        <th>Role</th>
                        <th>Appointer</th>
                        <th>Permissions</th>
                    </tr>
                </thead>
                <tbody>
                    {roles.map((role, index) => (
                        <TableRow key={`role-${index}`} role={role} index={index} />
                    ))}
                </tbody>
            </Table>
            <Modal show={showAppointeeModal} onHide={handleCloseAppointeeModal}>
    <Modal.Header closeButton>
        <Modal.Title>Add Appointee</Modal.Title>
    </Modal.Header>
    <Modal.Body>
        <Form>
            <Form.Group controlId="formUsername">
                <Form.Label>Username</Form.Label>
                <Form.Control type="text" placeholder="Enter username" value={memberUserName} onChange={(e) => setName(e.target.value)} />
            </Form.Group>
            <Form.Group controlId="formRole">
                <Form.Label>Role</Form.Label>
                <Form.Select value={roleName} onChange={(e) => setRole(e.target.value)}>
                <option value="Owner">Owner</option>
                <option value="Manager">Manager</option>
            </Form.Select>      
            </Form.Group>
            <Form.Group>
                <Form.Label></Form.Label>
                {Object.keys(Permission).map((perm: string) => (
                    <Form.Check
                        key={perm}
                        type="checkbox"
                        label={(permission.includes(perm) ? '✓ ' : '') + (Permission as Record<string, string>)[perm]} // Add a checkmark to the label if the permission is included in the state
                        checked={permission.includes(perm)}
                        onChange={(e) => {
                            const checkedPermission = perm;
                            let updatedPermissions = [...permission]; // Create a copy of the permissions array
                            
                            // Check if the permission is already included in the permissions array
                            const permissionIndex = updatedPermissions.indexOf(checkedPermission);
                            
                            // If the permission is already checked, remove it from the array
                            if (permissionIndex !== -1) {
                                updatedPermissions.splice(permissionIndex, 1);
                            } else {
                                // If the permission is not checked, add it to the array
                                updatedPermissions.push(checkedPermission);
                            }
                            
                            // Update the permissions state with the new array
                            setPermission(updatedPermissions);
                        }}
                        className="black-checkbox" // Add this line to apply the black-checkbox class
            />
        ))}
            </Form.Group>
        </Form>
    </Modal.Body>
    <Modal.Footer>
        <Button variant="secondary" onClick={handleCloseAppointeeModal}>Close</Button>
        <Button variant="primary" type="submit" onClick={handleAddAppointeeSubmit}>Submit</Button>
        </Modal.Footer>
</Modal>
        </>
    );
};

export default MyTable;

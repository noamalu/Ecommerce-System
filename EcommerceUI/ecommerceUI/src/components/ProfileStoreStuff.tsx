import React from 'react';
import { Table, Dropdown, Form } from 'react-bootstrap';
import { Role } from './ProfileStoreNav';
import { RiLockLine } from 'react-icons/ri'; // Importing lock icon

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
}

const MyTable: React.FC<MyTableProps> = ({ roles }) => {
    return (
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
    );
};

export default MyTable;

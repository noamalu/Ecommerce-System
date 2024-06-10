import React, { useEffect, useState } from 'react';
import { Container, Table } from 'react-bootstrap';

interface PurchaseHistoryProps {
    view: 'profileStoreNav' | 'purchaseHistory';
}

const PurchaseHistory: React.FC<PurchaseHistoryProps> = ({ view }) => {
    const [purchases, setPurchases] = useState<any[]>([]);

    useEffect(() => {
        if (view === 'purchaseHistory') {
            // Fetch purchase history data from the API
            const fetchPurchaseHistory = async () => {
                const tokenId = localStorage.getItem('tokenId'); // Retrieve the tokenId from localStorage
                //
                if (!tokenId) {
                    console.error('No tokenId found in localStorage');
                    return;
                }

                try {
                    //TODO change the url token id
                    const response = await fetch(`https://localhost:7163/api/Client/Member/PurchaseHistory?tokenId=${123}`, {
                        method: 'GET'
                    });
                    const data = await response.json();
                    if (response.ok) {
                        setPurchases(data.Value);
                    } else {
                        console.error('Error fetching purchase history:', data.ErrorMessage);
                    }
                } catch (error) {
                    console.error('Error occurred while fetching purchase history:', error);
                }
            };

            fetchPurchaseHistory();
        }
    }, [view]); // Adding view as a dependency

    return (
        <Container className="my-3">
            <h2>Purchase History</h2>
            <Table striped bordered hover className="my-3">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Item Name</th>
                        <th>Description</th>
                        <th>Price</th>
                        <th>Date Purchased</th>
                        <th>Quantity</th>
                    </tr>
                </thead>
                <tbody>
                    {purchases.map((purchase, index) => (
                        <tr key={index}>
                            <td>{index + 1}</td>
                            <td>{purchase.itemName}</td>
                            <td>{purchase.description}</td>
                            <td>{purchase.price}</td>
                            <td>{new Date(purchase.datePurchased).toLocaleDateString()}</td>
                            <td>{purchase.quantity}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </Container>
    );
};

export default PurchaseHistory;
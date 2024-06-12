import React, { useState } from 'react';
import { Table, Dropdown, Form, Stack, Button, Card, Row, Col, Container } from 'react-bootstrap';
import { Product } from './ProfileStoreNav'
import { RiSave2Fill } from 'react-icons/ri'; // Importing the shopping cart icon
import { getToken } from '../services/SessionService';


interface ProfileUpdateInventoryProps {
    products: Product[];
}

export const ProfileUpdateInventory: React.FC<ProfileUpdateInventoryProps> = ({ products }) => {
    console.log(products);
    return (
        <>
            <h2>Update Inventory</h2>
            <Container className="flex small-padding">
            { <Row  md={Math.min(products.length, 3)} className="vertical-center">
                
                {products.map((product, index) => (
                    <Col>
                        <ProductDetails key={index} product={product} />
                    </Col>
                ))}
                
            </Row> }
            </Container>
        </>
    );
};

interface ProductDetailsProps {
    product: Product;
}

const ProductDetails: React.FC<ProductDetailsProps> = ({ product }) => {
    const [productPrice, setProductPrice] = useState(product.productPrice);
    const [productQuantity, setProductQuantity] = useState(product.productQuantity);

    const handlePriceChange = (e : any) => {
        setProductPrice(e.target.value);
    }

    const handleQuantityChange = (e : any) => {
        setProductQuantity(e.target.value);
    }

    const updateProduct = async () => {
        const response= await fetch(`https://localhost:7163/api/Market/Store/${product.storeId}/Products/${product.productId}?identifier=${getToken()}`, {
           method: 'PUT',
           headers: {
               'Content-Type': 'application/json',
           },
           body: JSON.stringify(
            {   storeId: product.storeId,
                id: product.productId,
                productName: product.productName,
                productDescription: product.productDescription,
                price: productPrice,
                quantity: productQuantity,
           })  
       })
       if (response.ok) {
           alert('Item updated successfully');
       } else {
           const responseData = await response.json();
           console.error('Failed to update item:', responseData);
           alert('Failed to update item. Please try again later.');
       }
   };


    return (
        <>
            <Card style={{ width: '23rem' }}>
            <Card.Body>
            <Card.Title><b>{product.productName} </b></Card.Title>
            <Card.Text> {product.productDescription} </Card.Text>
            <Stack direction="horizontal" gap={3}>
                <Container>
                    <Row>
                        <Col> Price </Col>
                        <Col> <Form.Control
                                        type="number"
                                        className="mx-2"
                                        value={productPrice}
                                        onChange={handlePriceChange}
                                        min="0"
                                        style={{ width: '7vw', textAlign: 'center' }}
                            /> </Col>
                    </Row>
                    <Row>
                        <Col> Quantity </Col>
                        <Col>                     <Form.Control
                                        type="number"
                                        className="mx-2"
                                        value={productQuantity}
                                        onChange={handleQuantityChange}
                                        min="0"
                                        style={{ width: '7vw', textAlign: 'center' }}
                            /> </Col>
                    </Row>
                </Container>
                <Button variant="outline-info" onClick={updateProduct}> <RiSave2Fill size={20} /></Button>

            </Stack>
            </Card.Body>
            </Card>
        </>
    );
};

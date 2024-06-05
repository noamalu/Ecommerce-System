import React, {Component} from "react";
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import placeholder from '../assets/placeholder.jpg';

interface ItemCard {
    itemName: string;
    description: string;
    price: string;
    addToCart: (storeId: number, productId: number) => void;
    storeId: number;
    productId: number;
}

export const ItemCard: React.FC<ItemCard> = ({storeId, productId,itemName, description, price}) => {
        return (
            // <h1> card</h1>
            <Card style={{ width: '18rem' }}>
            <Card.Img variant="top" src={placeholder} />
            <Card.Body>
                <Card.Title>{itemName}</Card.Title>
                <Card.Text>
                {description}
                </Card.Text>
                <Card.Text>
                    {price}
                </Card.Text>
                <Button variant="primary" onClick={() => addToCart(storeId, productId)}>Add to cart</Button>
            </Card.Body>
            </Card>
        )
}

//make an add to cart request or something
const addToCart = async (storeId: number, productId: number) => {
    try {
        const tokenId = 123; //need to chnage token id
        const response = await fetch(`https://localhost:7163/api/Client/Cart?tokenId=${tokenId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          id: productId, // Assuming you have productId and other necessary data
          storeId: storeId,
          quantity: 1 // Assuming you're adding one item at a time
        })
      });
  
      if (response.ok) {
        const responseData = await response.json();
        console.log('Cart update success:', responseData);
        alert('Product added to cart successfully!');
      } else {
        console.error('Cart update failed:', response.statusText);
        alert('Failed to add product to cart. Please try again later.');
      }
    } catch (error) {
      console.error('Error occurred while adding to cart:', error);
      alert('An error occurred while processing your request. Please try again later.');
    }
  };
  

export default ItemCard;

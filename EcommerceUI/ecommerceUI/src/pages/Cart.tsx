import React, {Component, useState, useEffect} from "react";
import Table from 'react-bootstrap/Table';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';


const handleChange = (e, product: {storeId: number, id: number, productName: string, quantity: number}) => {
    const newQuantity = e.target.value;
    console.log(product.quantity);
    console.log(newQuantity);
    var multiplier = 1;
    if (newQuantity < product.quantity)
        multiplier = -1;

    var tokenId = 123;

    fetch(`https://localhost:7163/api/Client/Cart?tokenId=${tokenId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
                    storeId: product.storeId,
                    id: product.id,
                    productName: product.productName, //should not be neccessary
                    price: 0, //should not be neccessary
                    quantity: multiplier,
                  })
      }).then((r) => {
        if (r.ok) {
          alert("incrementing quantity worked")
          return;
        } else {
          throw new Error('incrementing quantity didnt work');
        }
      })
      .catch((error) => {
        window.alert(error.message);
      });

      window.location.reload(); //refresh the page to show updated items and quantities
};


export const Cart = () => {
    const [dataValue, setDataValue] = useState<any[]>([]);
    var tokenId = 123;

    //get cart information
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`https://localhost:7163/api/Client/Cart?tokenId=${tokenId}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Error occurred in getting cart information');
                }

                const data = await response.json();
                setDataValue(data.value);
                console.log(dataValue);
            } catch (error) {
                console.error('Error:', error);
            }
        };

        fetchData();
    });


    return (
        <div className="small-padding">
            <Button className="align-right" onClick={() => alert("clicked on purchase cart")}> Purchase cart </Button>
            <Table striped bordered hover className="my-3 full-width">
                <thead>
                    <tr>
                        <th>Store Name</th>
                        <th>Product Name</th>
                        <th>Price</th>
                        <th>Quantity</th>
                    </tr>
                </thead>
                <tbody>                
                    {dataValue.map((product) => (
                    <tr>
                    <td>{product.storeName}</td>
                    <td>{product.name}</td>
                    <td></td>
                    <td> <Form.Control
                        type="number"
                        className="mx-2"
                        value={product.quantity}
                        onChange={(e) => handleChange(e, {storeId: product.storeId, id: product.id, productName: product.name, quantity: product.quantity})}
                        min="0"
                        style={{ width: '60px', textAlign: 'center' }}
                    /> </td>
                </tr>
                ))}
                </tbody>
            </Table>
        </div>
    );
    
}
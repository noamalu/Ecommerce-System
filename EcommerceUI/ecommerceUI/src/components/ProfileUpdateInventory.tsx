import React, { useState } from 'react';
import { Table, Dropdown, Form } from 'react-bootstrap';
import { Product } from './ProfileStoreNav';

interface ProfileUpdateInventoryProps {
    products: Product[];
}

export const ProfileUpdateInventory: React.FC<ProfileUpdateInventoryProps> = ({ products }) => {
    return (
        <div>
            <h2>Update Inventory</h2>
            {products.map((product, index) => (
                <ProductDetails key={index} product={product} />
            ))}
        </div>
    );
};

interface ProductDetailsProps {
    product: Product;
}

const ProductDetails: React.FC<ProductDetailsProps> = ({ product }) => {
    const [productName, setProductName] = useState(product.productName);
    const [productPrice, setProductPrice] = useState(product.productPrice);
    const [productQuantity, setProductQuantity] = useState(product.productQuantity);
    const [productCategory, setProductCategory] = useState(product.productCategory);
    const [productDescription, setProductDescription] = useState(product.productDescription);
    const [productKeywords, setProductKeywords] = useState(product.productKeywords);
    const [productRating, setProductRating] = useState(product.productRating);
    const [ageLimit, setAgeLimit] = useState(product.ageLimit);
    const [sellMethod, setSellMethod] = useState(product.sellMethod);

    // Function to handle changes in product details
    const handleProductChange = (fieldName: string, value: any) => {
        switch (fieldName) {
            case 'productName':
                setProductName(value);
                break;
            case 'productPrice':
                setProductPrice(value);
                break;
            // Add cases for other fields as needed
            default:
                break;
        }
    };

    return (
        <div className="product-details">
            <h3>Product Details</h3>
            <table>
                <tbody>
                    <tr>
                        <td>Product Name:</td>
                        <td>
                            <input
                                type="text"
                                value={productName}
                                onChange={(e) => handleProductChange('productName', e.target.value)}
                            />
                        </td>
                    </tr>
                    <tr>
                        <td>Product Price:</td>
                        <td>
                            <input
                                type="number"
                                value={productPrice}
                                onChange={(e) => handleProductChange('productPrice', parseFloat(e.target.value))}
                            />
                        </td>
                    </tr>
                    {/* Add similar input fields for other product details */}
                </tbody>
            </table>
        </div>
    );
};

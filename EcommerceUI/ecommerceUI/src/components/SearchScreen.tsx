// import React from "react";
// import { Buttons } from "./Buttons";
// import { InputFields } from "./InputFields";
// import ProductDesktop from "./ProductDesktop";
// import "./search_style.css";

// export const SearchScreen = () => {
//     return (
//         <div className="home-screen">
//             <div className="div-2">
//                 <div className="frame-4">
//                     <div className="category">
//                         <div className="text-wrapper-4">1,201 items</div>
//                     </div>
//                     <div className="products">
//                         <div className="product-desktop-2">
//                             <div className="main-article-link-2">
//                                 <div className="label-2">
//                                     <div className="label-3">
//                                         <div className="text-wrapper-5">New Arrival</div>
//                                     </div>
//                                 </div>
//                                 <img className="image-2" alt="Image" src="image-2.svg" />
//                             </div>
//                             <div className="frame-5">
//                                 <div className="frame-6">
//                                     <div className="frame-7">
//                                         <div className="basic-jumper-with-2">product name&nbsp;&nbsp;01</div>
//                                         <img className="img-2" alt="Frame" src="frame-39-2.svg" />
//                                     </div>
//                                     <div className="frame-8">
//                                         <div className="swatches-2">
//                                             <div className="link-beige-wrapper">
//                                                 <div className="link-beige-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                         </div>
//                                     </div>
//                                 </div>
//                                 <div className="frame-9">
//                                     <div className="frame-10">
//                                         <div className="text-wrapper-6">$140.00</div>
//                                         <div className="text-wrapper-7">$220.00</div>
//                                     </div>
//                                 </div>
//                             </div>
//                         </div>
//                         <div className="product-desktop-2">
//                             <div className="main-article-link-2">
//                                 <div className="label-2">
//                                     <div className="label-3">
//                                         <div className="text-wrapper-5">New Arrival</div>
//                                     </div>
//                                 </div>
//                                 <img className="image-3" alt="Image" src="image-3.svg" />
//                             </div>
//                             <div className="frame-5">
//                                 <div className="frame-6">
//                                     <div className="frame-7">
//                                         <div className="basic-jumper-with-2">product name&nbsp;&nbsp;01</div>
//                                         <img className="img-2" alt="Frame" src="frame-39-3.svg" />
//                                     </div>
//                                     <div className="frame-8">
//                                         <div className="swatches-2">
//                                             <div className="link-beige-wrapper">
//                                                 <div className="link-beige-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                         </div>
//                                     </div>
//                                 </div>
//                                 <div className="frame-9">
//                                     <div className="frame-10">
//                                         <div className="text-wrapper-6">$140.00</div>
//                                         <div className="text-wrapper-7">$220.00</div>
//                                     </div>
//                                 </div>
//                             </div>
//                         </div>
//                         <div className="product-desktop-2">
//                             <div className="main-article-link-2">
//                                 <div className="label-2">
//                                     <div className="label-3">
//                                         <div className="text-wrapper-5">New Arrival</div>
//                                     </div>
//                                 </div>
//                                 <img className="image-4" alt="Image" src="image-4.svg" />
//                             </div>
//                             <div className="frame-5">
//                                 <div className="frame-6">
//                                     <div className="frame-7">
//                                         <div className="basic-jumper-with-2">product name&nbsp;&nbsp;01</div>
//                                         <img className="img-2" alt="Frame" src="frame-39-4.svg" />
//                                     </div>
//                                     <div className="frame-8">
//                                         <div className="swatches-2">
//                                             <div className="link-beige-wrapper">
//                                                 <div className="link-beige-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                             <div className="main-article-2">
//                                                 <div className="link-black-2" />
//                                             </div>
//                                         </div>
//                                     </div>
//                                 </div>
//                                 <div className="frame-9">
//                                     <div className="frame-10">
//                                         <div className="text-wrapper-6">$140.00</div>
//                                         <div className="text-wrapper-7">$220.00</div>
//                                     </div>
//                                 </div>
//                             </div>
//                         </div>
//                     </div>
//                     <div className="products">
//                         <ProductDesktop
//                             className="design-component-instance-node"
//                             divClassName="product-desktop-instance"
//                             frame="frame-39-5.svg"
//                             image="image-5.svg"
//                             property1="default"
//                             text="product name&nbsp;&nbsp;01"
//                         />
//                         <ProductDesktop
//                             className="design-component-instance-node"
//                             divClassName="product-desktop-instance"
//                             frame="frame-39-6.svg"
//                             image="image-6.svg"
//                             property1="default"
//                             text="product name&nbsp;&nbsp;01"
//                         />
//                         <ProductDesktop
//                             className="design-component-instance-node"
//                             divClassName="product-desktop-instance"
//                             frame="frame-39-7.svg"
//                             image="image-7.svg"
//                             property1="default"
//                             text="product name&nbsp;&nbsp;01"
//                         />
//                     </div>
//                 </div>
//                 <div className="footer">
//                     <div className="frame-11">
//                         <div className="frame-12">
//                             <div className="frame-13">
//                                 <p className="p">Be the first to know</p>
//                                 <div className="frame-14">
//                                     <InputFields
//                                         icon={false}
//                                         inputFieldsMainPlaceholder="Email Address"
//                                         state="default"
//                                         textBelow={false}
//                                     />
//                                     <Buttons
//                                         buttonsMainTextTitle="Join"
//                                         className="design-component-instance-node"
//                                         icon="no-icon"
//                                         size="small"
//                                         state="default"
//                                         type="text-link"
//                                     />
//                                 </div>
//                             </div>
//                             <div className="frame-15">
//                                 <div className="frame-16">
//                                     <div className="list-item-link-terms">Terms of Use</div>
//                                     <div className="list-item-link">Privacy Policy</div>
//                                     <div className="list-item-link-2">Imprint</div>
//                                 </div>
//                                 <div className="text-wrapper-8">copyright kennethcole.co.uk, 2024</div>
//                             </div>
//                         </div>
//                         <div className="list">
//                             <div className="item-link-customer">Customer Service</div>
//                             <div className="item-link-contact-us">Contact Us</div>
//                             <p className="item-link-gift-card">Gift Card &amp; Store Credit</p>
//                             <div className="item-link-payment">Payment</div>
//                             <div className="item-link-shipping">Shipping</div>
//                             <div className="item-link-returns">Returns &amp; Exchanges</div>
//                         </div>
//                         <div className="list">
//                             <div className="item-link-about-us">About us</div>
//                             <div className="item-link-press">Press &amp; Events</div>
//                             <div className="item-link-careers">Careers</div>
//                             <div className="item-link-investor">Investor Relations</div>
//                             <div className="item-link-affiliates">Affiliates</div>
//                             <div className="item-link-returns">FAQ</div>
//                         </div>
//                         <div className="div-footer-columns-wrapper">
//                             <div className="div-footer-columns">
//                                 <div className="text-wrapper-9">Follow us on</div>
//                                 <img className="frame-17" alt="Frame" src="frame-810.svg" />
//                             </div>
//                         </div>
//                     </div>
//                 </div>
//                 <div className="MENU">
//                     <div className="text-wrapper-10">logo</div>
//                     <div className="frame-18">
//                         <img className="img-2" alt="Frame" src="frame-1239.svg" />
//             <img className="bell" alt="Bell" src="bell.svg" />
//             <img className="line" alt="Line" src="line-316.svg" />
//             <img className="img-2" alt="Shopping bag" src="shopping-bag.svg" />
//           </div>
//           <img className="frame-19" alt="Frame" src="frame-1259.svg" />
//           <div className="text-wrapper-11">contact us</div>
//         </div>
//         <div className="frame-20">
//           <div className="dropdown">
//             <div className="text-wrapper-12">Filter By</div>
//             <div className="text-wrapper-12">clear all</div>
//           </div>
//           <div className="frame-21">
//             <div className="dropdown-2">
//               <div className="text-wrapper-12">Price</div>
//               <img className="vector" alt="Vector" src="vector.svg" />
//             </div>
//             <div className="dropdown-2">
//               <div className="text-wrapper-12">Rank</div>
//               <img className="vector" alt="Vector" src="vector-2.svg" />
//             </div>
//             <div className="dropdown-2">
//               <div className="text-wrapper-12">Category</div>
//               <img className="vector" alt="Vector" src="vector-3.svg" />
//             </div>
//           </div>
//         </div>
//         <div className="overlap-group">
//           <div className="tops-wrapper">
//             <div className="tops">TOPS</div>
//           </div>
//         </div>
//         <img className="line-2" alt="Line" src="line-317.svg" />
//         <div className="footer-2">
//           <div className="frame-11">
//             <div className="frame-12">
//               <div className="frame-13">
//                 <p className="p">Be the first to know</p>
//                 <div className="frame-14">
//                   <InputFields
//                     icon={false}
//                     inputFieldsMainPlaceholder="Email Address"
//                     state="default"
//                     textBelow={false}
//                   />
//                   <Buttons
//                     buttonsMainTextTitle="Join"
//                     className="design-component-instance-node"
//                     icon="no-icon"
//                     size="small"
//                     state="default"
//                     type="text-link"
//                   />
//                 </div>
//               </div>
//               <div className="frame-15">
//                 <div className="frame-16">
//                   <div className="list-item-link-terms">Terms of Use</div>
//                   <div className="list-item-link">Privacy Policy</div>
//                   <div className="list-item-link-2">Imprint</div>
//                 </div>
//                 <div className="text-wrapper-8">copyright kennethcole.co.uk, 2024</div>
//               </div>
//             </div>
//             <div className="list">
//               <div className="item-link-customer">Customer Service</div>
//               <div className="item-link-contact-us">Contact Us</div>
//               <p className="item-link-gift-card">Gift Card &amp; Store Credit</p>
//               <div className="item-link-payment">Payment</div>
//               <div className="item-link-shipping">Shipping</div>
//               <div className="item-link-returns">Returns &amp; Exchanges</div>
//             </div>
//             <div className="list">
//               <div className="item-link-about-us">About us</div>
//               <div className="item-link-press">Press &amp; Events</div>
//               <div className="item-link-careers">Careers</div>
//               <div className="item-link-investor">Investor Relations</div>
//               <div className="item-link-affiliates">Affiliates</div>
//               <div className="item-link-returns">FAQ</div>
//             </div>
//             <div className="div-footer-columns-wrapper">
//               <div className="div-footer-columns">
//                 <div className="text-wrapper-9">Follow us on</div>
//                 <img className="frame-22" alt="Frame" src="frame-810-2.svg" />
//               </div>
//             </div>
//           </div>
//         </div>
//         <img className="line-3" alt="Line" src="line-318.svg" />
//       </div>
//     </div>
//   );
// };
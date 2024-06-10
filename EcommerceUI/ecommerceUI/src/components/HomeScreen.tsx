// // src/components/HomeScreen.tsx
// import React from "react";
// import { Buttons } from "./Buttons";
// import { InputFields } from "./InputFields";
// import RegistrationForm from "./LoginForm";
// import "./style.css";

// interface HomeScreenProps {
//   onToggleForm: () => void;
//   showRegistrationForm: boolean;
// }

// export const HomeScreen: React.FC<HomeScreenProps> = ({ onToggleForm, showRegistrationForm }) => {
//   const handleButtonClick = async () => {
//     try {
//       const tokenId = 123; // Assuming you have a tokenId
//       const response = await fetch(`https://localhost:7163/api/Client/Guest/Register?tokenId=${tokenId}`, {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json"
//         },
//         body: JSON.stringify({
//           username: "username", 
//           password: "password",
//           email: "email",
//           age: "age"
//         })
//       });



//       if (!response.ok) {
//         throw new Error(`Failed to register: ${response.status}`);
//       }

//       const data = await response.json();
//       console.log("Response received:", data);
//     } catch (error) {
//       console.error("Error:", error);
//     }

    
//   };
  
//   const handleFrameClick = () => {
//     showRegistrationForm = true;
//   }
 

    

//   return (
//     <div className="home-screen">
//       <div className="div">
//         <footer className="footer">
//           <div className="frame">
//             <div className="frame-2">
//               <div className="frame-3">
//                 <p className="text-wrapper">Be the first to know</p>
//                 <div className="frame-4">
//                   <InputFields
//                     icon={false}
//                     inputFieldsMainPlaceholder="Email Address"
//                     state="default"
//                     textBelow={false}
                    
//                   />
//                   <Buttons
//                     buttonsMainTextTitle="Join"
//                     className="buttons-instance"
//                     icon="no-icon"
//                     size="small"
//                     state="default"
//                     type="text-link"
//                     onClick={handleButtonClick}
//                   />
//                 </div>
//               </div>
//               <div className="frame-5">
//                 <div className="frame-6">
//                   <div className="list-item-link-terms">Terms of Use</div>
//                   <div className="list-item-link">Privacy Policy</div>
//                   <div className="list-item-link-2">Imprint</div>
//                 </div>
//                 <div className="text-wrapper-2">copyright kennethcole.co.uk, 2024</div>
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
//                 <div className="text-wrapper-3">Follow us on</div>
//                 <img className="img" alt="Frame" src="frame-810.svg" />
//               </div>
//             </div>
//           </div>
//         </footer>
//         <div className="div-2">
//           <div className="div-2">
//             <div className="rectangle" />
//           </div>
//           <div className="MENU">
//             <div className="text-wrapper-4">logo</div>
//             <div className="frame-7">
//             <button className="show-registration-button" onClick={onToggleForm}>
              
//             </button>
//             {showRegistrationForm && <RegistrationForm />}
//               <div className="div-wrapper">
//                 <div className="text-wrapper-5">RS</div>
//               </div>
//               <img className="bell" alt="Bell" src="bell.svg" onClick={onToggleForm} style={{ cursor: 'pointer' }} />
//               <img className="line" alt="Line" src="line-316.svg" />
//               <img className="img" alt="Shopping bag" src="shopping-bag.svg" />
//             </div>
//             <img className="frame-8" alt="Frame" src="frame-1259.svg" />
//             <div className="text-wrapper-6">contact us</div>
//           </div>
//         </div>
//         <div className="frame-9">
//           <div className="category">
//             <div className="text-wrapper-7">1,201 items</div>
//           </div>
//           <div className="products">
//             {/* Product Cards */}
//           </div>
//         </div>
//         <img className="line-2" alt="Line" src="line-318.svg" />
//       </div>
//     </div>
//   );
// };



import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "./assets/vite.svg";
import heroImg from "./assets/hero.png";
import "./App.css";
import TreeNav from "./Components/TreeNav.jsx";
import { Layout } from "antd";
import Sider from "antd/es/layout/Sider.js";
import { Content } from "antd/es/layout/layout.js";
import { BrowserRouter, Route, Routes } from "react-router";
import { BOMDetail } from "./Components/BoMdetail.jsx";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <BrowserRouter>
        <Layout>
          <Sider width={200} className="site-layout-background">
            <TreeNav />
          </Sider>
          <Content>
            <Routes>
              <Route path="/" element={<div>Home Page</div>} />
              <Route path="/:bomId" element={<BOMDetail />} />
            </Routes>
          </Content>
        </Layout>
      </BrowserRouter>
    </>
  );
}

export default App;

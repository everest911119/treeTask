import { useEffect, useState } from "react";
import { useParams } from "react-router";
import axios from "axios";
import { message, Table } from "antd";
export const BOMDetail = () => {
  const params = useParams();
  const { bomId } = params;
  const [data, setData] = useState([]);
  useEffect(() => {
    const fetchData = async () => {
      const res = await axios.get(
        `http://localhost:5016/treetask/Boms/${bomId}`,
      );
      if (res.status !== 200) {
        message.error("Failed to fetch BOM details");
      }

      setData(res.data);
    };
    fetchData();
  }, [bomId]);
  const coloms = [
    {
      title: "componenT_NAME",
      dataIndex: "componenT_NAME",
      key: "componenT_NAME",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "parT_NUMBER",
      dataIndex: "parT_NUMBER",
      key: "parT_NUMBER",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "title",
      dataIndex: "title",
      key: "title",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "quantity",
      dataIndex: "quantity",
      key: "quantity",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "type",
      dataIndex: "type",
      key: "type",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "item",
      dataIndex: "item",
      key: "item",
      render: (text) => <a>{text}</a>,
    },
    {
      title: "material",
      dataIndex: "material",
      key: "material",
      render: (text) => <a>{text}</a>,
    },
  ];
  return (
    <>
      <div>Bom Detail Page for ID: {bomId}</div>
      <Table dataSource={data} columns={coloms} style={{ margin: "10px" }} />
    </>
  );
};

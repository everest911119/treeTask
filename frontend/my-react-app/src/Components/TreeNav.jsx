import React from "react";
import { Button, Tooltip, Tree } from "antd";
import axios from "axios";
import { message } from "antd";
import { useNavigate } from "react-router";
const MemoTooltip = React.memo(Tooltip);

// calculate all keys in the tree data for expanding all nodes
const collectAllKeys = (nodes) =>
  nodes.flatMap((node) => {
    return [node.key, ...(node.children ? collectAllKeys(node.children) : [])];
  });
// addKeys function to add unique keys to each node in the tree data
const addKeys = (nodes, round = null) =>
  nodes.map((node, index) => {
    const key = round === null ? `${index}` : `${round}-${index}`;
    return {
      ...node,
      key,
      children: node.children ? addKeys(node.children, key) : [],
    };
  });
const TreeNav = () => {
  const [data, setData] = React.useState([]);
  const navigate = useNavigate();
  React.useEffect(() => {
    const fetchData = async () => {
      const res = await axios.get("http://localhost:5016/treetask/Boms");
      if (res.status !== 200) {
        message.error("Failed to fetch BOM data");
        return;
      }
      const dataWithKeys = addKeys(res.data);
      setData(dataWithKeys);
    };
    fetchData();
  }, []);

  const [expandedKeys, setExpandedKeys] = React.useState([]);
  const handleExpandAll = () => {
    setExpandedKeys(collectAllKeys(data));
  };
  const handleOnExpand = (expandedKeysValue) => {
    setExpandedKeys(expandedKeysValue);
  };
  const handleSelect = (selectedKeys, { node }) => {
    if (node.children.length == 0) {
      message.info(`Selected leaf node: ${node.name}`);
    } else {
      navigate(`/${node.name}`);
    }
    console.log("onSelect", selectedKeys, node);
  };

  return (
    <div>
      <Button
        type="primary"
        style={{ marginBottom: 12 }}
        onClick={handleExpandAll}
        disabled={expandedKeys.length === collectAllKeys(data).length}
      >
        Expand All
      </Button>
      <Tree
        treeData={data}
        height={700}
        expandedKeys={expandedKeys}
        onExpand={handleOnExpand}
        onSelect={handleSelect}
        titleRender={(item, index) => {
          const title = item.name;
          return (
            <MemoTooltip key={item.key} title={title}>
              {title}
            </MemoTooltip>
          );
        }}
      />
    </div>
  );
};
export default TreeNav;

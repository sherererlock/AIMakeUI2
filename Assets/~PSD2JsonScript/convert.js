const psd2json = require('psd2json');
const path = require('path');

const psdFilePath = process.argv[2];

if (!psdFilePath) {
  console.error('请提供PSD文件的路径作为参数。');
  console.error('例如: node convert.js your_file.psd');
  process.exit(1);
}

const outputDir = path.dirname(psdFilePath);

try {
  // 这会将PSD文件转换为JSON，并将其保存在同一目录中。
  psd2json(psdFilePath, { outJsonDir: outputDir });
  
  const jsonFileName = path.basename(psdFilePath, '.psd') + '.json';
  const jsonFilePath = path.join(outputDir, jsonFileName);
  
  console.log(`成功将 ${psdFilePath} 转换为 ${jsonFilePath}`);
} catch (error) {
  console.error('转换PSD文件时出错:', error);
  process.exit(1);
}
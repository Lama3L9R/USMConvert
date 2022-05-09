# USMConvert

基于 .Net Core 6 和 VGMToolbox、ffmpeg 的 USM 文件转换工具。

A USM Convert tool based on .Net Core 6 and VGMToolbox, ffmpeg.

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

---

## 使用方法 | Usage

1. 下载预编译二进制文件 | Download pre-built binary executable
2. 下载 ffmpeg | Download ffmpeg
3. 执行指令 | Execute Command
```
$ USMConvert </Path/To/USMFile>
```

Windows用户也可以直接将文件拖拽到 USMConvert.exe 上

Windows users can also drag USM file to USMConvert.exe

## 其他平台用户 | Other platform user

目前来说我懒得配置 github flow 所以没有linux等其他文件的预编译二进制文件，请您clone仓库然后自己编译。

Currently there is no linux and other platform executable binary file because I'm lazy to configure Github flow.
Please clone this repository and build by your self.

```
$ git clone https://github.com/Lama3L9R/USMConvert
$ cd USMConvert
$ dotnet publish -p:PublishSingleFile=true -c Release --self-contained false -r <platform>-x86
```
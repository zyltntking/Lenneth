# CentOS7 FastDFS简单部署

tracker节点与data storage节点合一

## 一、基础环境：

* [CentOS 7.x](https://www.centos.org/download/)

docker pull centos:7

docker run --privileged --name fastdfs -p 9080:80 -p 22122:22122 -p 23000:23000 -ti centos:7 /bin/bash

>本部署方案系统采用CentOS最小安装方案，上文链接中任意镜像均包含该安装方案，初学者可用虚拟机先行模拟。

* 软件方案选择 "最小安装"
* 网络方案中开启以太网连接，并配置IP为静态IP，该IP作为节点IP使用
* 设置root账户密码（也可建立一个新用户）

## 二、安装FastDFS环境

#### 0.前言

>操作环境:CentOS 7.x
>
>集群方案:单机节点

软件的安装将在 `/usr/local/src` 目录下进行

FastDFS工作目录约定为 `/fastdfs`

所有`yum`软件的安装不加预确认标记，需要手动确认安装，可自行添加 `-y` 标记

① 升级CentOS内核和软件包

    yum update

② 安装辅辅助软件
    
    yum install net-tools wget tree zip unzip vim*
    
③ 安装依赖

    yum install cmake make gcc gcc-c++ pcre-devel zlib-devel openssl-devel

#### 1.安装 libfastcommon

>libfastcommon是从 FastDFS 和 FastDHT 中提取出来的公共 C 函数库

① 下载libfastcommon

    cd /usr/local/src
    wget https://github.com/happyfish100/libfastcommon/archive/V1.0.39.tar.gz
    
② 解压
    
    tar -zxvf V1.0.39.tar.gz
    cd libfastcommon-1.0.39
    
③ 编译，安装
    
    ./make.sh
    ./make.sh install

④ 创建软链接

    ln -s /usr/lib64/libfastcommon.so /usr/local/lib/libfastcommon.so
    ln -s /usr/lib64/libfastcommon.so /usr/lib/libfastcommon.so

#### 2.下载安装FastDFS

① 下载FastDFS
    
    cd /usr/local/src
    wget https://github.com/happyfish100/fastdfs/archive/V5.11.tar.gz

② 解压

    tar -zxvf V5.11.tar.gz
    cd fastdfs-5.11
    
③ 编译、安装

    ./make.sh
    ./make.sh install

④ 检查安装是否成功

>使用[`ls`](http://www.runoob.com/linux/linux-comm-ls.html)命令查看目录

>查看服务脚本目录

    ls -l /etc/init.d/fdfs*

>*具有如下目录：*
>
>*/etc/init.d/fdfs_storaged*
>
>*/etc/init.d/fdfs_trackerd*

>查看配置目录

    ls /etc/fdfs/

>*具有如下文件：*
>
>*client.conf.sample*
>
>*storage.conf.sample*
>
>*tracker.conf.sample*

>查看命令工具目录

    ls -l /usr/bin/fdfs*
    ls -l /usr/bin/*.sh
    
>*具有如下目录：*
>
>*/usr/bin/fdfs_appender_test*
>
>*/usr/bin/fdfs_appender_test1*
>
>*/usr/bin/fdfs_append_file*
>
>*/usr/bin/fdfs_crc32*
>
>*/usr/bin/fdfs_delete_file*
>
>*/usr/bin/fdfs_download_file*
>
>*/usr/bin/fdfs_file_info*
>
>*/usr/bin/fdfs_monitor*
>
>*/usr/bin/fdfs_storaged*
>
>*/usr/bin/fdfs_test*
>
>*/usr/bin/fdfs_test1*
>
>*/usr/bin/fdfs_trackerd*
>
>*/usr/bin/fdfs_upload_appender*
>
>*/usr/bin/fdfs_upload_file*
>
>及如下脚本
>
>*/usr/bin/restart.sh*
>
>*/usr/bin/stop.sh*

⑤ 创建软链接

    ln -s /usr/bin/fdfs_trackerd   /usr/local/bin
    ln -s /usr/bin/fdfs_storaged   /usr/local/bin
    ln -s /usr/bin/stop.sh         /usr/local/bin
    ln -s /usr/bin/restart.sh      /usr/local/bin

#### 3.配置FastDFS跟踪器(Tracker)

① 创建配置文件

    cd /etc/fdfs
    cp tracker.conf.sample tracker.conf
    vim tracker.conf
    
② [使用VIM](http://www.runoob.com/linux/linux-vim.html)编辑tracker.conf，查找并修改如下配置项，其它可保持默认

    # 配置文件是否不生效，false 为生效
    disabled=false
    
    # 提供服务的端口
    port=22122
    
    # Tracker 数据和日志目录地址(根目录必须存在,子目录会自动创建)
    base_path=/fastdfs/tracker
    
    # HTTP 服务端口 默认为8080
    http.server_port=8080

③ 创建tracker基础数据目录，即`base_path`对应的目录

    mkdir -p /fastdfs/tracker

④ [防火墙](http://www.firewalld.org/documentation/man-pages/)中打开跟踪端口(22122)

    yum install firewalld firewall-config
    systemctl start firewalld
    systemctl enable firewalld
    firewall-cmd --permanent --add-port=22122/tcp
    firewall-cmd --reload

⑤ 启动Tracker

    yum install initscripts

>初次成功启动，会在 `/fastdfs/tracker/` (配置的`base_path`)下创建 `data`、`logs` 两个目录

    service fdfs_trackerd start
    
查看 FastDFS Tracker 是否已成功启动，若22122端口正在被监听，则Tracker服务安装成功

    netstat -unltp|grep fdfs
    
>可使用`service fdfs_trackerd stop`关闭tracker服务

⑥ 设置Tracker开机启动
    
    chkconfig fdfs_trackerd on

⑦ 查看tracker server 是否成功创建工作目录

    cd /fastdfs/tracker
    tree
    
#### 4.配置FastDFS存储(Storage)

① 创建配置文件

>集群部署时，可把storage与tracker分开部署

    cd /etc/fdfs
    cp storage.conf.sample storage.conf
    vim storage.conf
    
② [使用VIM](http://www.runoob.com/linux/linux-vim.html)编辑storage.conf，查找并修改如下配置项，其它可保持默认

>单机节点下注意tracker_server需要修改为CentOS内网地址

    # 配置文件是否不生效，false 为生效
    disabled=false 
    
    # 指定此 storage server 所在 组(卷)
    group_name=group1
    
    # storage server 服务端口
    port=23000
    
    # 心跳间隔时间，单位为秒 (这里是指主动向 tracker server 发送心跳)
    heart_beat_interval=30
    
    # Storage 数据和日志目录地址(根目录必须存在，子目录会自动生成)
    base_path=/fastdfs/storage
    
    # 存放文件时 storage server 支持多个路径。这里配置存放文件的基路径数目，通常只配一个目录。
    store_path_count=1
    
    
    # 逐一配置 store_path_count 个路径，索引号基于 0。
    # 如果不配置 store_path0，那它就和 base_path 对应的路径一样。
    store_path0=/fastdfs/file
    
    # FastDFS 存储文件时，采用了两级目录。这里配置存放文件的目录个数。 
    # 如果本参数只为 N（如： 256），那么 storage server 在初次运行时，会在 store_path 下自动创建 N * N 个存放文件的子目录。
    subdir_count_per_path=256
    
    # tracker_server 的列表 ，会主动连接 tracker_server
    # 有多个 tracker server 时，每个 tracker server 写一行
    tracker_server=192.168.16.137:22122
    
    # 允许系统同步的时间段 (默认是全天) 。一般用于避免高峰同步产生一些问题而设定。
    sync_start_time=00:00
    sync_end_time=23:59
    # 访问端口 默认8888
    http.server_port=80

③ 创建Storage基础数据目录，对应上文配置

    mkdir -p /fastdfs/storage
    mkdir -p /fastdfs/file

④ [防火墙](http://www.firewalld.org/documentation/man-pages/)中打开存储器端口(23000)

    firewall-cmd --permanent --add-port=23000/tcp
    firewall-cmd --reload

⑤ 启动 Storage

>启动Storage前确保Tracker是启动的。初次启动成功，会在 storage工作目录下创建 data、logs 两个目录

    service fdfs_storaged start

查看 Storage 是否成功启动，若23000端口正在被监听，则 Storage 启动成功

    netstat -unltp|grep fdfs

>可使用`service fdfs_storaged stop`关闭storage服务

查看Storage和Tracker是否在通信

    /usr/bin/fdfs_monitor /etc/fdfs/storage.conf

>ip_addr= ***** 后面若显示ACTIVE 表示存储节点和跟踪器正在通信

⑥ 设置 Storage 开机启动

    chkconfig fdfs_storaged on
    
⑦ Storage 目录

>同 Tracker，Storage 启动成功后，在base_path 下创建了data、logs目录，记录着 Storage Server 的信息

    cd /fastdfs/storage
    tree
    cd /fastdfs/file/data
    ls
    
#### 5.文件上传测试

① 创建客户端配置文件

>集群部署时可用独立客户端进行测试

    cd /etc/fdfs
    cp client.conf.sample client.conf
    vim client.conf

② [使用VIM](http://www.runoob.com/linux/linux-vim.html)编辑client.conf，查找并修改如下配置项，其它可保持默认

>单机节点下注意tracker_server需要修改为CentOS内网地址

    # Client 的数据和日志目录
    base_path=/fastdfs/client
    
    # Tracker路径
    tracker_server=192.186.16.137:22122

③ 创建客户端工作目录，对应上文配置

    mkdir -p /fastdfs/client
    
④ 上传测试

>需要确保根目录下存在一张图片，这里的图片为2018.jpg

在linux内部执行如下命令上传 2018.jpg 

    cd ~
    /usr/bin/fdfs_upload_file /etc/fdfs/client.conf 2018.jpg

>上传成功返回文ID，返回的文件ID由group、存储目录、两级子目录、fileid、文件后缀名（由客户端指定，主要用于区分文件类型）拼接而成
>
>`group1/M00/00/00/wKgQiVq7K8-AbuYbAAUo9f1ek7g799.jpg`
>
>其中
>* `group1 --> 组名`
>* `M00 --> 磁盘名`
>* `00/00 --> 存储目录`
>* `wKgQiVq7K8-AbuYbAAUo9f1ek7g799.jpg --> 文件名`

## 三、安装Nginx

>安装Nginx作为服务器以支持Http方式访问文件
>
>同时，FastDFS的Nginx模块也需要Nginx环境
>
>集群部署时Nginx只需要安装到StorageServer所在的服务器即可，用于访问文件

#### 1.安装Nginx
    
① 下载Nginx

    cd /usr/local/src
    wget -c https://nginx.org/download/nginx-1.14.2.tar.gz

② 解压

    tar -zxvf nginx-1.14.2.tar.gz
    cd nginx-1.14.2
    
③ 使用默认配置

>nginx编译[配置](http://nginx.org/en/docs/configure.html)

    ./configure
    
④ 编译、安装

    make
    make install
    
⑤ 启动nginx

    cd /usr/local/nginx/sbin/
    ./nginx

>其他nginx命令
>
>./nginx -s stop
>
>./nginx -s quit
>
>./nginx -s reload

⑥ 查看nginx的版本及模块

    /usr/local/nginx/sbin/nginx -V

⑦ 设置nginx开机启动

    vim /etc/rc.local
    
在末尾添加一行

    /usr/local/nginx/sbin/nginx
    
保存后修改执行权限

    chmod 755 /etc/rc.local

⑧ [防火墙](http://www.firewalld.org/documentation/man-pages/)中打开nginx端口(80)

    firewall-cmd --permanent --add-port=80/tcp
    firewall-cmd --reload

#### 2.配置文件访问

>通过nginx代理使用http协议访问文件

① 修改[nginx.conf](http://nginx.org/en/docs/beginners_guide.html)

    vim /usr/local/nginx/conf/nginx.conf
    

添加一个location，将 /group1/M00 映射到 /fastdfs/file/data

    #########################################################
    # redirect server error pages to the static page /50x.html
    #
    error_page   500 502 503 504  /50x.html;
    location = /50x.html {
         root   html;
    }
    #########################################################
    ##添加下文中的内容
    location /group1/M00 {
        alias /fastdfs/file/data;
    }
    
重启nginx

    /usr/local/nginx/sbin/nginx -s reload
 
② 在浏览器访问之前上传的图片

    http://192.168.16.137/group1/M00/00/00/wKgQiVq7K8-AbuYbAAUo9f1ek7g799.jpg

## 四、FastDFS 配置 Nginx 模块   

#### 1.安装配置Nginx模块

① fastdfs-nginx-module 模块说明

>FastDFS 通过 Tracker 服务器，将文件放在 Storage 服务器存储， 但是同组存储服务器之间需要进行文件复制， 有同步延迟的问题。
>
>假设 Tracker 服务器将文件上传到了 192.168.16.137，上传成功后文件 ID已经返回给客户端。
>
>此时 FastDFS 存储集群机制会将这个文件同步到同组存储 192.168.16.100，在文件还没有复制完成的情况下，客户端如果用这个文件 ID 在 192.168.16.100 上取文件,就会出现文件无法访问的错误。
>
>而 fastdfs-nginx-module 可以重定向文件链接到源服务器取文件，避免客户端由于复制延迟导致的文件无法访问错误。

② 下载 fastdfs-nginx-module

    cd /usr/local/src
    wget https://github.com/happyfish100/fastdfs-nginx-module/archive/V1.20.zip
    
③ 解压

    unzip V1.20.zip //fastdfs-nginx-module-1.20

④ 重命名

    mv fastdfs-nginx-module-1.20  fastdfs-nginx-module-master
    
⑤ 配置Nginx

在nginx中添加模块
 
先停掉nginx服务   
    
    /usr/local/nginx/sbin/nginx -s stop

进入nginx解压目录

    cd /usr/local/src/nginx-1.14.2/

添加模块

    ./configure --add-module=../fastdfs-nginx-module-master/src

重新编译、安装

    cd /usr/local/src/fastdfs-nginx-module-master/src/

    vim config

    ngx_module_incs="/usr/include/fastdfs /usr/include/fastcommon/"

    CORE_INCS="$CORE_INCS /usr/include/fastdfs /usr/include/fastcommon/"

    make && make install
    
⑥ 查看Nginx的模块

    /usr/local/nginx/sbin/nginx -V

>若输出`configure arguments: --add-module=../fastdfs-nginx-module-master/src` 则配置成功

⑦ 复制并修改 fastdfs-nginx-module 源码中的配置文件到/etc/fdfs 目录

    cd /usr/local/src/fastdfs-nginx-module-master/src
    cp mod_fastdfs.conf /etc/fdfs/
    vim /etc/fdfs/mod_fastdfs.conf

⑧ [使用VIM](http://www.runoob.com/linux/linux-vim.html)编辑mod_fastdfs.conf，查找并修改如下配置项，其它可保持默认

>单机节点下注意tracker_server需要修改为CentOS内网地址
>
>集群部署时需要修改为主节点

    # 连接超时时间
    connect_timeout=10
    
    # Tracker Server
    tracker_server=192.168.16.139:22122
    
    # StorageServer 默认端口
    storage_server_port=23000
    
    # 如果文件ID的uri中包含/group**，则要设置为true
    url_have_group_name = true
    
    # Storage 配置的store_path0路径，必须和storage.conf中的一致
    store_path0=/fastdfs/file

⑨ 复制 FastDFS 的部分配置文件到/etc/fdfs 目录

    cd /usr/local/src/fastdfs-5.11/conf/
    cp anti-steal.jpg http.conf mime.types /etc/fdfs/
    
⑩ 修改nginx.conf

    vim /usr/local/nginx/conf/nginx.conf
    
>修改配置，其它的默认

在80端口下添加fastdfs-nginx模块

    #########################################################
    # redirect server error pages to the static page /50x.html
    #
    error_page   500 502 503 504  /50x.html;
    location = /50x.html {
             root   html;
    }
        
    ##注释下文中的内容
    #location /group1/M00 {
    #        alias /fastdfs/file/data;
    #}
    #########################################################
    #添加下文中的内容
    location ~/group([0-9])/M00 {
        ngx_fastdfs_module;
    }

>注意：
>
>listen 80 端口值是要与 /etc/fdfs/storage.conf 中的 http.server_port=80 (前面改成80了)相对应。如果改成其它端口，则需要统一，同时在防火墙中打开该端口。
>
>location 的配置，如果有多个group则配置location ~/group([0-9])/M00 ，没有则不用配group。

⑪ 在/fastdfs/file 文件存储目录下创建软连接，将其链接到实际存放数据的目录，一般情况下这一步可以省略

    ln -s /fastdfs/file/data/ /fastdfs/file/data/M00

⑫ 启动nginx

    /usr/local/nginx/sbin/nginx

>若输出`ngx_http_fastdfs_set pid=***`则配置成功

⑬ 在地址栏访问

    http://192.168.16.137/group1/M00/00/00/wKgQiVq7K8-AbuYbAAUo9f1ek7g799.jpg
    
>与直接使用nginx路由访问不同的是，这里配置 fastdfs-nginx-module 模块，可以重定向文件链接到源服务器获取文件

>集群结构图如下

![结构图](img/infrastructure.png)

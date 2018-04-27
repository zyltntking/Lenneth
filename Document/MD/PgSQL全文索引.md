升级内核
	
	yum update
	
安装辅助软件

    	yum install net-tools wget git zip unzip bzip2 vim*
	
安装依赖

	yum install cmake gcc gcc-c++ pcre-devel zlib-devel openssl-devel
	
安装并配置postgresql
	
	更新社区源
	rpm -Uvh https://download.postgresql.org/pub/repos/yum/9.6/redhat/rhel-7-x86_64/pgdg-redhat96-9.6-3.noarch.rpm
	安装postgresql[postgresql<version>*]
	yum list postgresql96* postgresql-unit96*
	yum install postgresql96* postgresql-unit96*
	
	初始化数据库：
	/usr/pgsql-9.6/bin/postgresql96-setup initdb
	
	开放防火墙
	firewall-cmd --permanent --add-port=5432/tcp
	firewall-cmd --permanent --add-port=80/tcp
	firewall-cmd --permanent --add-port=22/tcp
	firewall-cmd --reload
	#firewall-cmd --permanent --zone=public --list-ports
	启动服务
	systemctl start postgresql-9.6.service
	设置开机启动
	systemctl enable postgresql-9.6.service
	
	切换到postgres用户
	su - postgres
	进入psql命令环境
	psql
	修改postgres用户密码
	\password postgres 
	退出psql命令环境
	\q
	切换回root用户
	su - root
	
	配置数据库认证模式
	vim /var/lib/pgsql/9.6/data/pg_hba.conf
	修改如下(自行配置可访问的ip)
	# TYPE  DATABASE        USER            ADDRESS                 METHOD
	local   all             all                                     md5
	host    all             all             127.0.0.1/32            md5
	host    all             all             0.0.0.0/0               md5
	host    all             all             ::1/128                 md5
	重启服务
	systemctl restart postgresql-9.6

	配置TCP/IP连接
	vim /var/lib/pgsql/9.6/data/postgresql.conf
	修改如下
	listen_addresses = '*'
	port = 5432
	重启服务
	systemctl restart postgresql-9.6
	
	安装中文分词引擎
	cd /usr/local/src
	wget -q -O - http://www.xunsearch.com/scws/down/scws-1.2.1.tar.bz2 | tar xjf -
	cd scws-1.2.1 ; ./configure ; make install
	
	安装postgresql 分词扩展
	cd /usr/local/src
	git clone https:github.com/amutu/zhparser.git
	cd zhparser
	export PATH=/usr/pgsql-9.6/bin:$PATH
	which pg_config
	make & make install
	
	修改分词配置
	vim /var/lib/pgsql/9.6/data/postgresql.conf
	
	在文档末尾添加
	<该配置相当于scws复合分词等级为15(1+2+4+8)>
	#忽略所有的标点等特殊符号: 
	zhparser.punctuation_ignore = fales
	#闲散文字自动以二字分词法聚合: 
	zhparser.seg_with_duality = fales
	#将词典全部加载到内存里: 
	zhparser.dict_in_memory = fales
	#短词复合:1 
	zhparser.multi_short = true
	#散字二元复合: 2
	zhparser.multi_duality = true
	#重要单字复合: 4
	zhparser.multi_zmain = true
	#全部单字复合: 8
	zhparser.multi_zall = false
	
	systemctl restart postgresql-9.6
	
	fulltext配置
	查看安装目录</usr/pgsql-9.6/>
	rpm -ql postgresql96
	添加各种扩展
	<通过 ll /usr/pgsql-9.6/share/extension/ 查看已安装未加载扩展>
	su - postgres
	psql
	CREATE EXTENSION "uuid-ossp";
	CREATE EXTENSION "adminpack";
	CREATE EXTENSION zhparser;
	CREATE TEXT SEARCH CONFIGURATION testzhcfg (PARSER = zhparser);
	ALTER TEXT SEARCH CONFIGURATION testzhcfg ADD MAPPING FOR n,v,a,i,e,l WITH simple;
	\q
	su - root
	
	创建数据库
	CREATE DATABASE "Test"
    WITH 
    OWNER = postgres
    TEMPLATE = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'zh_CN.UTF-8'
    LC_CTYPE = 'zh_CN.UTF-8'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

	COMMENT ON DATABASE "Test"
    IS '测试全文索引';
	
	创建示例表
	CREATE TABLE public."Document"
	(
		id uuid NOT NULL DEFAULT uuid_generate_v4(),
		title character varying(255) COLLATE pg_catalog."zh_CN.utf8" NOT NULL DEFAULT '默认标题',
		content text COLLATE pg_catalog."zh_CN.utf8" NOT NULL DEFAULT '默认内容',
		matedata json DEFAULT '{"author":"默认作家"}',
		searchvector tsvector,
		PRIMARY KEY (id)
	)
	WITH (
		OIDS = FALSE
	)
	TABLESPACE pg_default;

	ALTER TABLE public."Document"
		OWNER to postgres;
	COMMENT ON TABLE public."Document"
		IS '文档表';

	COMMENT ON COLUMN public."Document".id
		IS '文章标识';

	COMMENT ON COLUMN public."Document".title
		IS '标题';

	COMMENT ON COLUMN public."Document".content
		IS '内容';

	COMMENT ON COLUMN public."Document".matedata
		IS '文章元数据';
		
	#UPDATE public."Document" SET searchvector = to_tsvector('testzhcfg', coalesce(title || ' ' ||content,''));
	
	CREATE INDEX fts_inx_gin_zhcn ON public."Document" USING GIN(searchvector);
	
	CREATE FUNCTION messages_trigger() RETURNS trigger AS $$
	begin
	  new.searchvector :=
		 setweight(to_tsvector('testzhcfg', coalesce(new.title,'')), 'A') ||
		 setweight(to_tsvector('testzhcfg', coalesce(new."content",'')), 'D');
	  return new;
	end
	$$ LANGUAGE plpgsql;

	CREATE TRIGGER searchvector_trigger BEFORE INSERT OR UPDATE ON "Document" FOR EACH ROW EXECUTE PROCEDURE messages_trigger();

	导入数据 略

	DROP INDEX fts_inx_gin_zhcn;

	CREATE INDEX fts_inx_gin_zhcn ON public."Document" USING GIN(searchvector);

	搜索测试
	SELECT COUNT(1) FROM "Document"
	> OK
	> 时间: 0.001s
	1031
	
	SELECT * FROM "Document" LIMIT 1
	> OK
	> 时间: 0.109s

	SELECT "id" FROM "Document" WHERE "content" LIKE '%医学%'
	> OK
	> 时间: 13.087s

	SELECT "id" FROM "Document" WHERE searchvector @@ '医学'
	> OK
	> 时间: 0.151s




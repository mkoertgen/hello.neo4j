# rsync cygpath weirdness, cf.: http://micahasmith.github.io/2015/01/22/coreos-vagrant-windows-file-share/
ENV["VAGRANT_DETECTED_OS"] = ENV["VAGRANT_DETECTED_OS"].to_s + " cygwin"

Vagrant.configure("2") do |config|
  # CoreOS is great for Docker, cf.: 
  # - https://github.com/coreos/coreos-vagrant
  # - https://serversforhackers.com/getting-started-with-docker/
  # - https://coreos.com/os/docs/latest/booting-on-vagrant.html
  config.vm.box = "coreos-stable"

  # release channels: alpha, stable
  # version: <specific>, current
  # cf.: https://github.com/coreos/coreos-vagrant/blob/master/Vagrantfile
  config.vm.box_url = "http://stable.release.core-os.net/amd64-usr/current/coreos_production_vagrant.json"
  config.vm.box_check_update = false

  # neo4j port mapping
  config.vm.network "forwarded_port", guest: 7474, host: 7474

  # docker is preinstalled on CoreOS
  #config.vm.provision :docker

  # cf.: 
  # - http://micahasmith.github.io/2015/01/22/coreos-vagrant-windows-file-share/
  # - http://docs.vagrantup.com/v2/synced-folders/rsync.html
  config.vm.synced_folder ".", "/vagrant", type: "rsync"

  if ENV['HTTP_PROXY']
    # proxy configuration
    if Vagrant.has_plugin?("vagrant-proxyconf")
      config.proxy.http     = "http://proxy:3128/"
      config.proxy.https    = "http://proxy:3128/"
      config.proxy.no_proxy = "localhost,127.0.0.1"
    else
      print "  WARN: Missing plugin 'vagrant-proxyconf'.\n"
      print "  Use 'vagrant plugin install vagrant-proxyconf' to install.\n"
      print "  You might need to do 'set HTTP_PROXY=http://proxy:3128' to do this, however.\n"
    end   
  end
  
  if Vagrant.has_plugin?("vagrant-cachier")
    # enable package caches
    # machine scope, cf.: http://fgrehm.viewdocs.io/vagrant-cachier/usage/
    config.cache.scope = :machine
  else
    print "  WARN: Missing plugin 'vagrant-cachier'.\n"
    print "  Use 'vagrant plugin install vagrant-cachier' to install.\n"
  end  
  
  if Vagrant.has_plugin?("vagrant-vbguest")
    config.vbguest.auto_update = false
  end

  # docker-compose, cf.: 
  # - https://github.com/leighmcculloch/vagrant-docker-compose
  if !Vagrant.has_plugin?("vagrant-docker-compose")
    print "  WARN: Missing plugin 'vagrant-docker-compose'.\n"
    print "  Use 'vagrant plugin install vagrant-docker-compose' to install.\n"
  end  
  
  # - http://stackoverflow.com/questions/30314808/docker-compose-does-not-install-or-run-properly-on-boot2docker/31442366#31442366
  config.vm.provision :shell, inline: "mkdir -p /opt/bin"
  config.vm.provision :docker_compose, 
    #compose_version: "1.3.2",
    #executable_install_path: "/opt/bin/docker-compose", 
    yml: "/vagrant/docker-compose.yml", 
    #rebuild: true, 
    project_name: "hello.neo4j",
    run: "always"  
end
